using IronPython.Hosting;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Net;

namespace SampleApp.script
{
    public partial class Engine : ComponentBase
    {
        public SfMaskedTextBox maskedTextBox;//インプット
        public string question;//入力要求メッセージ
        public string console;//コンソール出力内容
        public string script;
        /// <summary>
        /// script内のものを実行可能形態に変換
        /// </summary>
        void Ignition()
        {
            console += DateTime.Now.ToString("HH:mm:ss");
            console += "\r\nIgnition....";
            //やること
            /*
                スペース4つをtab文字化
                入力スクリプトの挿入
                出力スクリプトの挿入                      
             */
            script = script.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t");

        }
        /// <summary>
        /// script内のものを実行可能形態に変換
        /// </summary>
        /// <param name="raw_script">スクリプト</param>
        void Ignition(string raw_script)
        {
            script = raw_script;
            Ignition();
        }

        void Ignition(SfMaskedTextBox raw_script)
        {
            /*
             やってること
            HTMLを文字列化
            Pタグを改行に変更
            その他のタグを削除
            80byteルール守ってないけど許して
             */
            script = Regex.Replace(WebUtility.HtmlDecode(raw_script.Value.Replace("&nbsp;&nbsp;&nbsp;&nbsp;", "\t")).Replace("</p>", "\r\n"), "<[^>]*?>", ""); 
            Ignition();
        }
    }

}
