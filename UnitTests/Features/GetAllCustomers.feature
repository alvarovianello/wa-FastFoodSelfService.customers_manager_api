Feature: Buscar todos os clientes
  Como consumidor de API
  Quero recuperar todos os clientes
  Para que eu possa ver a lista de clientes

  Scenario: recuperar todos os clientes com sucesso
    Given que a API tem os seguintes clientes
      | Id | Name      | Cpf        | Email               |
      | 1  | John Doe  | 12345678901| johndoe@example.com |
      | 2  | Jane Doe  | 98765432100| janedoe@example.com |
    When solicito a todos os clientes
    Then a resposta deve conter 2 clientes
    And a resposta deverá incluir um cliente com Cpf "12345678901"
