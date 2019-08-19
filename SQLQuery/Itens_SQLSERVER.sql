USE [transparencia]
GO

/****** Object:  Table [dbo].[ItemLicitacao]    Script Date: 13/08/2019 09:26:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ItemLicitacao](
	[Número Licitação] [varchar](max) NULL,
	[Número Processo] [varchar](max) NULL,
	[Código Órgão] [varchar](max) NULL,
	[Nome Órgão] [varchar](max) NULL,
	[Código UG] [varchar](max) NULL,
	[Nome UG] [varchar](max) NULL,
	[CNPJ Vencedor] [varchar](max) NULL,
	[Nome Vencedor] [varchar](max) NULL,
	[Número Item] [varchar](max) NULL,
	[Descrição] [varchar](max) NULL,
	[Quantidade Item] [varchar](max) NULL,
	[Valor Item] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO