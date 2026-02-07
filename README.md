# GameHype
API REST para recomendação de jogos multiplataformas com bases nas preferências e requisitos técnicos informados.


# Proposta
O sistema foi desenvolvido para:
  - Recomendas jogos de acordo com um ou mais gêneros, plataforma (PC e/ou browser, opcionais) e RAM disponível (opcional), através do consumo da Free-To-Play Games Database API (https://www.freetogame.com/api-doc);
  - Retornar uma recomendação aleatória (nome e link) entre os jogos que atendam os critérios;
  - Salvar os jogos recomendados em banco de dados (utilizando ORM);
  - Disponibilizar um endpoint para consulta do histórico de recomendações;
  - Validar entradas e tratamentos adequados de erros;
  - Retornar uma mensagem apropriada quando nenhum jogo atende aos critérios informados.


# Como executar
É necessário ter o .NET SDK, na versão 8.0 ou superior.

## Passo a passo
Após clonar o repositório, abra a solução e execute o projeto GameHype.WebAPI. 
O banco de dados (GameHype.db) será criado na primeira execução. 
A interfce de usuário abrirá no navegador (Swagger).

# Endpoins da API
POST /api/recommendations
- Retorna uma recomendação aleatória de jogo conforme os filtros informados.
- Parâmetros:
    * genres (string obrigatória, no mínimo 1) - lista de gêneros. Ex: "mmo", "shooter";
    * platform (string opcional) -: tipo de plataforma do jogo. Ex: "pc", "browser" ou "all";
    * ramMb (inteiro opcional) - memória RAM disponível, em MB. Ex: 4096;

GET /api/recommendations
- Retorna o histórico de jogos recomendados e persistidos.

# Exemplos de uso
1 - Consulta sem informar plataforma e RAM:
    {
        "genres": 
        [
        "mmo","shooter"
      ],
      "platform": "",
      "ramMb": 0
    }  
 
 Retorna jogo escolhido aleatoriamente de qualquer plataforma e requisito:     
      200 - Response body          
      {
        "title": "Steel Legions",
        "url": "https://www.freetogame.com/open/steel-legions"      
      }

2 - Consulta sem informar gênero:
    {
      "genres": [
        ""
      ],
      "platform": "all",
      "ramMb": 16000
    }

Retorna erro:
      400 - Response body          
      "Informe, ao menos, um gênero."
