using System.Diagnostics;

namespace OOP_RPG.MSTests
{
    /// <summary>
    /// MS Testで使用するユーティリティライブラリー
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// debeg.writeline()によるdebugトレース出力を監視を開始する。<br/>
        /// この戻り値をswに代入した場合、
        /// sw.to_string()でdebugトレース出力を抽出できるようになる。
        /// </summary>
        /// <returns>stringwriter sw</returns>
        public static StringWriter DebugTraceListener()
        {
            StringWriter sw = new();
            TextWriterTraceListener lsnr = new(sw);
            Trace.Listeners.Add(lsnr);
            return sw;
        }
    }
}
