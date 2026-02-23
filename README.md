# Kedu Payment Plans API

Este projeto é a solução para o **Desafio Prático - Desenvolvedor C#**, focado no gerenciamento de planos de pagamento, responsáveis financeiros e cobranças no contexto educacional.

O projeto foi construído utilizando **.NET 8** e **PostgreSQL**, entregando **absolutamente todos os requisitos obrigatórios** listados no desafio, além de implementar com sucesso **todos os diferenciais (plus)** desejados.

---

## 🎯 Cobertura do Desafio

### Requisitos Obrigatórios Entregues (✔️ 100%)
1. **Cadastro de Responsável Financeiro**: Criação e consultas de responsáveis.
2. **Cadastro de Centro de Custo**: Implementado modelo customizável via API (Plus).
3. **Restrições Estruturais**: Plano possui 1 Responsável e 1 Centro de Custo. Cobranças com métodos rigorosos simulados (`BOLETO` com "linha digitável", `PIX` com "Chave EMV").
4. **Cálculos Automáticos**: Regra de Vencimento (`EstaVencida`) calculada em tempo real (não persistida). Valor Total derivado da somatória das cobranças.
5. **Pagamento**: Endpoints para transição segura de Status (emitida -> paga). Cobranças canceladas não aceitam pagamento.
6. **Consultas (REST)**: Total de planos, listagem de planos/cobranças por responsável (com paginação e filtros detalhados), e total numérico de cobranças vinculadas.

### Diferenciais / Extras Implementados (🌟 100%)
- ⭐ **API de Centros de Custo Customizáveis**: CRUD completo para `CentroDeCusto` no lugar de simples enums (solicitado no PDF como _Diferencial_).
- ⭐ **Gateway GraphQL**: Foram expostas _todas_ as operações do sistema também na interface GraphQL (Queries e Mutations), possuindo exata paridade de recursos com a interface REST (solicitado no PDF como _Diferencial_).
- 🏗️ **Clean Architecture**: Divisão purista em camadas (`Domain`, `Application`, `Infrastructure`, `API`), utilizando os princípios SOLID.
- 🧪 **Testes Unitários**: Integração com `xUnit`, `Moq` e `FluentAssertions` validando fluxos de serviço e construção de entidades.
- 🐳 **Docker Completo**: O projeto roda liso via `docker-compose`, unindo a API Rest/GraphQL ao banco PostgreSQL.

---

## 🛠️ Stack Tecnológico

- **Framework**: .NET 8 (ASP.NET Core Web API)
- **Database**: PostgreSQL 16
- **ORM**: Entity Framework Core 8 (`AsNoTracking` otimizado em queries de leitura)
- **GraphQL**: HotChocolate 13
- **Testes**: xUnit + Moq + FluentAssertions
- **Containerização**: Docker & Docker Compose
- **Documentação API**: Swagger (Swashbuckle)

---

## ⚙️ Como Rodar a Aplicação

A forma testável mais imediata é utilizando **Docker**. Garanta que seu SO possua o *Docker Desktop* ou *Docker Engine* ativo.

### Opção 1: Tudo Automatizado via Docker Compose (Recomendado)
Abre um terminal na raiz do projeto e emita a instrução abaixo. O comando vai "buildar" a imagem da API em .NET 8 e criar o container do Postgres lado a lado.
```bash
docker-compose up --build -d
```

### Opção 2: Localmente pelo CLI (.NET Run)
Se quiser rodar a API direto pelo seu terminal, inicie apenas o banco e use o SDK (também requer que porta `5432` esteja devidamente exposta no localhost para o BD).
```bash
docker-compose up postgres -d
dotnet run --project src/Kedu.API
```

> **NOTA:** A aplicação aplica automagicamente os *Migrations* do Entity Framework e cria as tabelas assim que o serviço é iniciado (comportamento injetado no `Program.cs`), você não precisa rodar scripts de DB manualmente! Adicionalmente, foi implementado um **Seed Inicial** contendo dados mockados (Centros de Custo, Responsáveis e Planos) exclusivamente para **facilitar e agilizar o teste da aplicação** pelos avaliadores.

---

## � Testando a Aplicação (Endpoints & Acessos)

Com a arquitetura levantada, você poderá explorar o desafio por 3 vias:

1. **Swagger (REST API UI)**: [http://localhost:8080/swagger](http://localhost:8080/swagger)
2. **GraphQL Playground**: [http://localhost:8080/graphql](http://localhost:8080/graphql)
3. **Client Simulado (cURL / Postman)**: Conecte em `http://localhost:8080`

> 💡 **Dica de Teste Rápido:** Salvei na raiz deste projeto o arquivo `Kedu Payment Plans API.postman_collection.json`. Você pode simplesmente importá-lo no seu **Postman** para ter acesso imediato a **todas as requisições REST** e **todas as 11 operações GraphQL** !

### 📝 Fluxo de Teste Sugerido Exigido no Desafio (cURL)

**1. Entrar com um Centro de Custo:**
```bash
curl -X 'POST' \
  'http://localhost:8080/centros-de-custo' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "nome": "Mensalidade do Ensino Médio",
  "descricao": "Turmas Matutinas"
}'
```
> 👉 *Copie o `id` que retornar na chave de resposta*.

**2. Entrar com o Responsável Financeiro:**
```bash
curl -X 'POST' \
  'http://localhost:8080/responsaveis' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "nome": "João Silva",
  "email": "joao.silva@teste.com",
  "telefone": "11999999999",
  "cpfCnpj": "52998224725"
}'
```
> 👉 *Copie o `id` da chave do Responsável.*

**3. Registrar o Plano e Gerar Cobranças:**
Troque os campos `<ID_RESPONSAVEL>` e `<ID_CENTRO>` para testar o relacionamento criado nas rotas 1 e 2!

```bash
curl -X 'POST' \
  'http://localhost:8080/planos-de-pagamento' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "responsavelId": "<ID_RESPONSAVEL>",
  "centroDeCustoId": "<ID_CENTRO>",
  "cobrancas": [
    {
      "valor": 600.00,
      "dataVencimento": "2026-06-10T00:00:00Z",
      "metodoPagamento": "PIX"
    },
    {
      "valor": 600.00,
      "dataVencimento": "2026-07-10T00:00:00Z",
      "metodoPagamento": "BOLETO"
    }
  ]
}'
```
> 👉 *Veja na resposta que o sistema auto-calculou o `valorTotal`. Copie um dos arrays de ID das `Cobrancas` injetadas*.

**4. Efetivar o Pagamento de uma Cobrança:**
Altere a `url` para preencher com o ID de uma das cobranças que foram criadas no bloco acima.
```bash
curl -X 'POST' \
  'http://localhost:8080/cobrancas/<ID_DA_COBRANCA>/pagar' \
  -H 'accept: text/plain' \
  -d ''
```

---

## 🧪 Rodando os Testes Automatizados

O software conta com suítes de testes englobando os fluxos das Entidades e os comportamentos dos Serviços (Application Layer).

Para validar via linha de comando:
```bash
dotnet test tests/Kedu.UnitTests/Kedu.UnitTests.csproj
```
Todos os casos cobrem comportamentos exigidos de falha e de sucesso.

Aprendi muita coisa desenvolvendo esse desafio agradeço pela oportunidade. Aproveite a API! 🚀
