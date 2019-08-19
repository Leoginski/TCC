USE [transparencia]
GO

/****** Object:  Table [dbo].[Licitacao]    Script Date: 13/08/2019 09:26:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Licitacao](
	[N�mero Licita��o] [varchar](max) NOT NULL,
	[N�mero Processo] [varchar](max) NULL,
	[Objeto] [varchar](max) NULL,
	[Modalidade Compra] [varchar](max) NULL,
	[Situa��o Licita��o] [varchar](max) NULL,
	[C�digo �rg�o Superior] [varchar](max) NULL,
	[Nome �rg�o Superior] [varchar](max) NULL,
	[C�digo �rg�o] [varchar](max) NULL,
	[Nome �rg�o] [varchar](max) NULL,
	[C�digo UG] [varchar](max) NULL,
	[Nome UG] [varchar](max) NULL,
	[Munic�pio] [varchar](max) NULL,
	[Data Publica��oDOU] [varchar](max) NULL,
	[Data Abertura] [varchar](max) NULL,
	[Valor Licita��o] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO