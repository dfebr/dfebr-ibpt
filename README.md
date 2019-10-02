# DFeBR.IBPT

**Oque é**

DFeBR.IBPT é uma lib escrita em .NET Standard cujo o objetivo é simplificar a busca de alíquotas IBPT por NCM, afim de agilizar e facilitar o processo de cálculo de impostos para documentos fiscais.

**A quem se destina**

A todos os desenvolvedores de NFC-e e NF-e na plataforma .NET

**Como instalar**

  **Install-Package DFeBR.IBPT -Version 1.0.0** | ou mais alto

**Funcionamento**

A lib realiza as consultas em um arquivo de dados SQLite3.
Esse arquivo deve ser gerado utilizando a ferramenta de linha de comando **"IbptGen.exe"** disponível no diretório do seu projeto após a instalação via NuGet.

Voce precisa ter em mãos o arquivo CSV **oficial** obtido através do site www.deolhonoimposto.ibpt.org.br.

Com o arquivo, execute a ferramenta de linha de comando da seguinte maneira:

**ATENÇÃO:**

Somente serão suportados os arquivos **OFICIAIS** obtidos no site acima. Tenha certeza de que os CSV's foram obtidos da fonte correta e confiável para garantir o bom funcionamento da DLL.

  **IbptGen.exe --gen "caminho do CSV"**
  
Após executar o comando, a ferramenta devolverá o arquivo .sqlite3 no mesmo diretório onde foi executada.
Com o arquivo de dados (sqlite3) em mãos, podemos começar a codificar.

**Realizando buscas de aliquotas com o DFeBR.IBPT**

Esta tarefa é incrivelmente simples:

```C#
   BuscaIBPT busca = new BuscaIBPT(@".\IBPT.sqlite3"); //caminho do arquivo de dados
   AliquotaIBPT aliquota = busca.Buscar("01013000"); //NCM do produto
```

Caso a consulta tenha sucesso, voce terá o objeto "AliquotaIBPT" com as seguintes propriedades:

NCM (string)

AliquotaFederal (decimal)

AliquotaEstadual (decimal)

AliquotaMunicipal (decimal)

AliquotaImportado (decimal)

Versao (string)

