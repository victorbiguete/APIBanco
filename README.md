# API SimplificaPay
## Apresentação

A API a seguir foi desenvolvida para o bootcamp Sharp Coders 2023 e construida pelo grupo SimplificaTech. O objetivo dessa API visa simular em um ambiente web um sistema bancário contendo funções como criar um cadastro, abri sua conta, realizar transações como saque, deposito, pagamento e extrato.

Esse projeto encontra-se em fase inicial de planejamento podendo realizar as tarefas basicas acima relaciadas, porém temos expectativas de aumentar sua funcionalida implementando uma aréa administrativa em que funcionarios da empresa(banco) poderão registrar novos funcionarios e até mesmo produtos para comercialização com futuros clientes.

## Desenvolvimento

A API foi construida em cima da arquitetura .Net utilizamos portanto a linguagem C# bem como o Entity Framework para questões de gerenciamento de dados. Utilizamos como ferramentas de armazenamento o SQlite visto que futuramente, conforme avanço do projeto, realizaremos o deploy do projeto bem como armazenamento remoto dos dados. Em questão de segurança utilizamos tokens JWT para autenticar acesso a paginas que necessitam de autorização e vinculo com o usuario logado para registros caso seja necessario a realização de auditorias.

## Integração

Como mencionado anteriormente, esse projeto visa criar um ambiente similar com os sites bancarios, portanto para termos um ambiente mais fiel possivel é necessario que outro projeto, esse frontend, rode junto com essa API para podermos ter um ambiente mais fiel possivel, o link você encontra aqui: [Frontend ImaPay](https://github.com/victorbiguete/ImaPayFrontEnd)

## Considerações Futuras

A API SimplificaPay é um esboço do que planejamos para o futuro que é criar um ambiente que simule na mais fidelidade possivel um ambiente bancario para isso estamos seguindo os seguintes passos:

1. Criação dos Requisitos Básicos (Criar conta, Saque, Depósito e Extrato)
2. Criação de um ambiente administrativo
3. Criação dos Produtos para agregar valor ao negócio
4. Conformidade com leis de segurança de dados.
5. Integração com I.A's para recomendação de produtos que sejam mais proximos possiveis do perfil do cliente.
   
## Agradecimentos e Menções

Gostariamos de agradecer a oportunidade do Bootcamp SharpCoders 2023 por agregar tamanho conhecimento em nossas vidas e um agradecimento especial as empresas ImãTech e MxM Sistemas que puderam proporcionar tamanha oportunidade em nossas vidas tanto em conhecimentos da aréa de tecnologia quanto de networking com demais participantes.

## Colaboradores

Esse projeto foi idealizado, criado e desenvolvido pela equipe SimplificaTech, grupo esse formado pelos membros:

1. Monitor - Victor Hugo Nunes Biguete - [GitHub](https://github.com/victorbiguete)
2. Dandara Silva - [GitHub](https://github.com/DandaraF)
3. Julio Hebert Silva - [GitHub](https://github.com/mmerga)
4. Ranna Pansera de Freitas - [GitHub](https://github.com/Rannatpf)


