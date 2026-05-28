# NIDUS – Smart Incubation

### *Tecnologia que choca o futuro* 🥚📡

## 📖 Sobre o Projeto

O **NIDUS** é uma solução de incubação inteligente desenvolvida para monitorar e automatizar incubadoras de diferentes espécies animais, trazendo mais segurança, precisão e controle para produtores e fazendas.

No agronegócio, pequenas oscilações de temperatura ou umidade podem causar grandes prejuízos, incluindo a perda completa de ninhadas de animais raros ou de alta produção. Pensando nisso, criamos um ecossistema IoT capaz de monitorar incubadoras em tempo real e agir automaticamente diante de situações críticas.

O sistema transforma incubadoras convencionais em dispositivos conectados, permitindo acompanhamento remoto através de uma plataforma web moderna e intuitiva.

---

## 🚀 Objetivo

Garantir maior taxa de sucesso na incubação de ovos através de:

* Monitoramento em tempo real;
* Controle automatizado do ambiente;
* Alertas instantâneos;
* Gestão centralizada de múltiplas incubadoras e fazendas;
* Regras inteligentes específicas para cada tipo de animal.

---

## 🧠 Diferenciais do NIDUS

O grande diferencial do projeto é sua inteligência adaptativa.

A plataforma identifica automaticamente o tipo de incubação e aplica regras específicas para cada espécie:

* 🐦 **Aves:** controle da frequência de rotação dos ovos;
* 🦎 **Répteis:** controle do substrato úmido e estabilidade térmica;
* 🌡️ Ajustes automáticos de temperatura e umidade;
* 🚨 Sistema de alertas em tempo real caso algo saia do esperado.

Além disso, o hardware pode atuar automaticamente para corrigir problemas antes que os ovos sejam comprometidos.

---

# 🛠️ Tecnologias Utilizadas

## 🔌 Hardware (IoT)

O sistema embarcado utiliza:

* **ESP32 com Wi-Fi**
* Sensor **DHT22** para temperatura e umidade
* Sensor **LDR** para luminosidade
* **LEDs** simulando atuadores:
  * Aquecedor
  * Umidificador
  * Luminosidade

O ESP32 realiza a leitura contínua dos sensores e envia os dados para a plataforma.

Link da simulação: https://wokwi.com/projects/465195256226022401

---

## 📡 Comunicação e Mensageria

A comunicação ocorre através do protocolo **MQTT**, permitindo transmissão leve e rápida dos dados IoT.

Os dados são enviados para a arquitetura **FIWARE**, utilizando:

* **Orion Context Broker**
* Gerenciamento de estados da incubadora
* Atualização em tempo real das informações

---

## ⚙️ Back-end

O back-end foi desenvolvido utilizando conceitos de:

* **Programação Orientada a Objetos (POO)**
* **Herança** para separar regras de cada espécie
* API REST para comunicação entre sistema e hardware
* Segurança utilizando **BCrypt**
* Persistência de dados em banco **SQL**

O sistema armazena históricos de monitoramento e eventos críticos.

---

## 💻 Front-end

A plataforma web possui:

* Dashboards interativos;
* Atualização em tempo real;
* Gráficos dinâmicos;
* Comunicação assíncrona utilizando **Ajax**;
* Interface responsiva e intuitiva.

O produtor consegue monitorar sem precisar atualizar a página.

---

# 📊 Funcionalidades

✅ Monitoramento de temperatura
✅ Monitoramento de umidade
✅ Monitoramento de luminosidade
✅ Alertas automáticos
✅ Controle inteligente por espécie
✅ Histórico de dados
✅ Dashboard em tempo real
✅ Gestão de múltiplas incubadoras
✅ Automação de atuadores

---

# 🏗️ Arquitetura do Sistema

```text
Sensores → ESP32 → MQTT → FIWARE → API Back-end → Banco SQL → Plataforma Web
```

---

# 🎯 Impacto do Projeto

O NIDUS reduz perdas na incubação, aumenta a eficiência operacional e traz automação inteligente para o agronegócio, permitindo que o produtor foque no crescimento do negócio em vez do monitoramento manual constante.

---

# 👨‍💻 Integrantes

| Nome | RM |
|---|---|
| Cauã Oliveira | 081240033 |
| Giovanna Bruna | 081240010 |
| Giovanna Franguelli | 081240019 |
| Isabella Apolinario | 081240025 |
| Júlia Pereira | 081240034 |
