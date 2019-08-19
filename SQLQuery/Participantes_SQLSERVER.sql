USE [transparencia]
GO

/****** Object:  Table [dbo].[ParticipanteLicitacao]    Script Date: 13/08/2019 09:26:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ParticipanteLicitacao](
	[N�mero Licita��o] [varchar](max) NULL,
	[N�mero Processo] [varchar](max) NULL,
	[C�digo �rg�o] [varchar](max) NULL,
	[Nome �rg�o] [varchar](max) NULL,
	[C�digo UG] [varchar](max) NULL,
	[Nome UG] [varchar](max) NULL,
	[C�digo Item Compra] [varchar](max) NULL,
	[Descri��o Item Compra] [varchar](max) NULL,
	[CNPJ Participante] [varchar](max) NULL,
	[Nome Participante] [varchar](max) NULL,
	[Flag Vencedor] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO