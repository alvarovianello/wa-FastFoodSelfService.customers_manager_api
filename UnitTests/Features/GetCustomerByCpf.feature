Feature: Buscar Cliente por CPF
  Como um usuário da API
  Eu quero buscar informações de um cliente pelo CPF
  Para obter seus detalhes caso ele exista

  Scenario: Buscar cliente com CPF existente
    Given um cliente com CPF "40851368875" existe no sistema
    When eu buscar pelo CPF "40851368875"
    Then o cliente retornado deve ter o nome "Álvaro Oliveira"

  Scenario: Buscar cliente com CPF inexistente
    Given nenhum cliente com CPF "98765432109" existe no sistema
    When eu buscar pelo CPF "98765432109"
    Then nenhum cliente deve ser retornado