# GameHype
API REST para recomendação de jogos multiplataformas com bases nas preferências e requisitos técnicos informados.


# Proposta
O desenvolvimento consiste em criar uma API que:
  - Recomende jogos de acordo com um ou mais gêneros, plataforma (PC e/ou browser, opcionais) e RAM disponível (opcional);
  - Consuma a Free-To-Play Games Database API (https://www.freetogame.com/api-doc) para buscar os jogos que atendam os parâmetros informados.
  - Retorne uma recomendação aleatória (nome e link) entre os jogos que atendam os critérios;
  - Persista os jogos recomendados em banco de dados (utilizando ORM);
  - Disponibiliza um endpoint para consulta do histórico de recomendações;
  - Realiza validação de entrada e tratamento adequado de erros;
  - Retorna uma mensagem apropriada quando nenhum jogo atende aos critérios informados.
