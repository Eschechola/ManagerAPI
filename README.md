# ManagerAPI
<p>Projeto criado na série de vídeos ensinando a construir uma API Rest utilizando .NET 5, EF Core, e boas práticas de arquitetura!</p>
<br>
<p>Aulas:</p>

<ol>
  <li><a href="https://www.youtube.com/watch?v=TovPavZjxOw&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&ab_channel=LucasEschechola">#0 - Introdução</a></li>
  <li><a href="https://www.youtube.com/watch?v=YGsb6P-w4aE&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=3&ab_channel=LucasEschechola">#1 - Definindo a Estrutura do Projeto</a></li>
  <li><a href="https://www.youtube.com/watch?v=FLz4p9aZvHI&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=4&ab_channel=LucasEschechola">#2 - Modelando Nossas Entidades</a></li>
  <li><a href="https://www.youtube.com/watch?v=1EYmcp9_4v4&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=5&ab_channel=LucasEschechola">#3.1- Iniciando a Camada de Infraestrutura e o Repository Pattern</a></li>
  <li><a href="https://www.youtube.com/watch?v=qzYaGXeYYtk&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=6&ab_channel=LucasEschechola">#3.2 - Finalizando a Camada de infraestrutura</a></li>
  <li><a href="https://www.youtube.com/watch?v=47TjQ6Dv29o&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=7&ab_channel=LucasEschechola">#4 - Construindo Nossa Camada de Serviço</a></li>
  <li><a href="https://www.youtube.com/watch?v=S9c2rZWYiVY&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=8&ab_channel=LucasEschechola">#5.1 - Criando Nossa Camada de API</a></li>
  <li><a href="https://www.youtube.com/watch?v=B6aOYL2SExA&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=9&ab_channel=LucasEschechola">#5.2 - Adicionando JWT a Nossa API</a></li>
  <li><a href="https://www.youtube.com/watch?v=4wd2lkOObRQ&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=10&ab_channel=LucasEschechola">#6 - Encerramento</a></li>
  <li><a href="https://www.youtube.com/watch?v=_-Fgyv-v060&list=PLdhhExru1TXcTTm-Mpfg2tN5B_rOTNvzy&index=10&ab_channel=LucasEschechola">#7 - [BONUS] Aumentando a Segurança da API</a></li>
  <li><a href="https://www.youtube.com/watch?v=u0W3zkTmgDA&t=71s">#8 - Iniciando nosso banco de dados no Azure!</a></li>
  <li><a href="https://www.youtube.com/watch?v=E5V71NKHwIU">#8.1 - Configurando o Azure Key Vault e realizando o deploy</a></li>
</ol>

<br><br>
<br>

<h3 align="center">Para poder rodar o projeto você precisa configurar algumas variaveis de ambiente</h3>
<br>
<p>Iniciar os segredos de usuários</p>
<pre>
dotnet user-secrets init
</pre>
<br>
<br>
<p>Configurar a string de conexão ao banco de dados</p>
<br>
<pre>
dotnet user-secrets set "ConnectionStrings:USER_MANAGER" "[STRING CONNECTION]"
</pre>
<br>
<br>
<p>Configurar dados de autenticação (JWT)</p>
<br>
<pre>
dotnet user-secrets set "Jwt:Key" "[JWT CRYPTOGRAPHY KEY]"
dotnet user-secrets set "Jwt:Login" "[JWT LOGIN]"
dotnet user-secrets set "Jwt:Password" "[JWT PASSWORD]"
</pre>
<br>
<br>
<p>Por fim você configura a chave de criptografia da aplicação</p>
<br>
<pre>
dotnet user-secrets set "Cryptography" "[CHAVE DE CRIPTOGRAFIA DA APLICAÇÃO]"
</pre>
<br>
<br>
<br>
<h3 align="center">Alguns comandos que podem ser úteis :)</h3>
<br>
<br>
<p>Listar todas os segredos de usuário da aplicação.</p>
<br>
<pre>
dotnet user-secrets list
</pre>
<br>
<br>
<p>Deletar um segredo de usuário da aplicação.</p>
<br>
<pre>
dotnet user-secrets remove "[CHAVE]"
</pre>
<br><br>
<br><br>
<br><br>
<p align="center">2021&copy;</p>
