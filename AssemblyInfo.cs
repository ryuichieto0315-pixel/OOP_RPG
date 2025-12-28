using System.Runtime.CompilerServices;

//MSTest から internal クラスのアクセスを受け入れるための設定
[assembly: InternalsVisibleTo("OOP_RPG.xUnitTests")]

// internal クラスにモックを注入するための設定
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]