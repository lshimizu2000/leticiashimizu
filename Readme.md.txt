# Help Desk MVC com ADO.NET

Sistema web para gerenciamento de chamados de suporte, desenvolvido com ASP.NET Core MVC, C#, SQL Server e ADO.NET.

## Funcionalidades

- Cadastro de chamados
- Listagem de chamados
- Consulta de detalhes
- Edição de prioridade e status
- Exclusão de chamados
- Validação de formulários

## Tecnologias utilizadas

- C#
- ASP.NET Core MVC
- ADO.NET
- Microsoft.Data.SqlClient
- SQL Server
- Bootstrap

## Arquitetura

O projeto utiliza ASP.NET Core MVC e Repository Pattern.

```text
Controller → Repository → ADO.NET → SQL Server