using System.Net.Sockets;
using System.Text;

namespace NidusFront.Controllers
{
    /// <summary>
    /// Publica comandos MQTT direto no broker, sem biblioteca externa.
    /// Usa um cliente TCP simples para montar o pacote MQTT 3.1.1.
    /// </summary>
    public class FiwareService
    {
        // =====================================================================
        // CONFIGURAÇÕES DO BROKER — ajuste se mudar o servidor
        // =====================================================================
        private const string BrokerHost = "3.229.16.6";
        private const int BrokerPort = 1883;

        // Tópico que o ESP32 assina (deve bater com TOPICO_COMMAND no .ino)
        private const string TopicCommand = "/TEF/incubadora1/cmd";

        // =====================================================================
        // MÉTODO PÚBLICO
        // =====================================================================

        /// <summary>
        /// Envia um comando para o ESP32.
        /// </summary>
        /// <param name="ligar">true = ligar a incubadora / false = desligar</param>
        public void EnviarComando(bool ligar)
        {
            // Payload esperado pelo callback() do ESP32:
            //   "incubadora1@ligada|true"  → liga
            //   "incubadora1@ligada|false" → desliga
            string payload = ligar
                ? "incubadora1@ligada|true"
                : "incubadora1@ligada|false";

            PublicarMqtt(TopicCommand, payload);
        }

        // =====================================================================
        // MQTT 3.1.1 — cliente TCP manual (sem pacote NuGet extra)
        // =====================================================================

        private static void PublicarMqtt(string topic, string payload)
        {
            using var tcp = new TcpClient();

            // Timeout de 5 s para não travar a requisição web
            tcp.Connect(BrokerHost, BrokerPort);
            tcp.SendTimeout = 5000;
            tcp.ReceiveTimeout = 5000;

            using var stream = tcp.GetStream();

            // --- CONNECT ---
            byte[] connectPacket = BuildConnect("nidus_webapp_" + Guid.NewGuid().ToString("N")[..6]);
            stream.Write(connectPacket, 0, connectPacket.Length);

            // Lê CONNACK (4 bytes) e verifica retorno
            byte[] connack = new byte[4];
            stream.Read(connack, 0, 4);
            if (connack[3] != 0x00)
                throw new Exception($"MQTT CONNACK erro: código {connack[3]}");

            // --- PUBLISH (QoS 0, sem resposta necessária) ---
            byte[] publishPacket = BuildPublish(topic, payload);
            stream.Write(publishPacket, 0, publishPacket.Length);

            // --- DISCONNECT ---
            stream.Write(new byte[] { 0xE0, 0x00 }, 0, 2);
        }

        // ---- Monta pacote CONNECT ----------------------------------------
        private static byte[] BuildConnect(string clientId)
        {
            byte[] clientIdBytes = Encoding.UTF8.GetBytes(clientId);
            byte[] protocolBytes = Encoding.UTF8.GetBytes("MQTT");

            // Payload: 2-byte length prefix + clientId bytes
            byte[] payload = new byte[2 + clientIdBytes.Length];
            payload[0] = (byte)(clientIdBytes.Length >> 8);
            payload[1] = (byte)(clientIdBytes.Length & 0xFF);
            Array.Copy(clientIdBytes, 0, payload, 2, clientIdBytes.Length);

            // Variable header (10 bytes para MQTT 3.1.1)
            byte[] varHeader = new byte[]
            {
                0x00, (byte)protocolBytes.Length,   // protocol name length
                (byte)'M',(byte)'Q',(byte)'T',(byte)'T',
                0x04,   // protocol level = 4 (MQTT 3.1.1)
                0x02,   // connect flags: Clean Session
                0x00, 0x3C  // keep-alive = 60 s
            };

            int remainingLength = varHeader.Length + payload.Length;

            var packet = new List<byte> { 0x10 };  // CONNECT fixed header
            packet.AddRange(EncodeRemainingLength(remainingLength));
            packet.AddRange(varHeader);
            packet.AddRange(payload);
            return packet.ToArray();
        }

        // ---- Monta pacote PUBLISH (QoS 0) ----------------------------------
        private static byte[] BuildPublish(string topic, string message)
        {
            byte[] topicBytes = Encoding.UTF8.GetBytes(topic);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // Topic: 2-byte length prefix + topic bytes
            byte[] topicSection = new byte[2 + topicBytes.Length];
            topicSection[0] = (byte)(topicBytes.Length >> 8);
            topicSection[1] = (byte)(topicBytes.Length & 0xFF);
            Array.Copy(topicBytes, 0, topicSection, 2, topicBytes.Length);

            int remainingLength = topicSection.Length + messageBytes.Length;

            var packet = new List<byte> { 0x30 };  // PUBLISH, QoS 0, no retain
            packet.AddRange(EncodeRemainingLength(remainingLength));
            packet.AddRange(topicSection);
            packet.AddRange(messageBytes);
            return packet.ToArray();
        }

        // ---- Codifica o campo "Remaining Length" (protocolo MQTT) ----------
        private static byte[] EncodeRemainingLength(int length)
        {
            var result = new List<byte>();
            do
            {
                byte encoded = (byte)(length % 128);
                length /= 128;
                if (length > 0) encoded |= 0x80;
                result.Add(encoded);
            } while (length > 0);
            return result.ToArray();
        }
    }
}