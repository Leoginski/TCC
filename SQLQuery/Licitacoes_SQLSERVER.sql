USE [transparencia]
GO

/****** Object:  Table [dbo].[Licitacao]    Script Date: 13/08/2019 09:26:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Licitacao](
	[Número Licitação] [varchar](max) NOT NULL,
	[Número Processo] [varchar](max) NULL,
	[Objeto] [varchar](max) NULL,
	[Modalidade Compra] [varchar](max) NULL,
	[Situação Licitação] [varchar](max) NULL,
	[Código Órgão Superior] [varchar](max) NULL,
	[Nome Órgão Superior] [varchar](max) NULL,
	[Código Órgão] [varchar](max) NULL,
	[Nome Órgão] [varchar](max) NULL,
	[Código UG] [varchar](max) NULL,
	[Nome UG] [varchar](max) NULL,
	[Município] [varchar](max) NULL,
	[Data PublicaçãoDOU] [varchar](max) NULL,
	[Data Abertura] [varchar](max) NULL,
	[Valor Licitação] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO