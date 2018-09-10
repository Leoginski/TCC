CREATE TABLE participantes (
codigo_orgao varchar(255) DEFAULT '',
nome_orgao varchar(255) DEFAULT '',
codigo_ug varchar(255) DEFAULT '',
nome_ug varchar(255) DEFAULT '',
numero_licitacao varchar(255) DEFAULT '',
periodo_licitacao date NOT NULL,
codigo_item varchar(255) DEFAULT '',
descricao_item varchar(255) DEFAULT '',
cnpj_participante varchar(255) DEFAULT '',
nome_participante varchar(255) DEFAULT '',
flag varchar(255) DEFAULT '') ENGINE = InnoDB;
