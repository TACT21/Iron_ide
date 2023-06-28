using BlazorWorker.Core;
using BlazorWorker.WorkerBackgroundService;
using BlazorWorker.BackgroundServiceFactory;
using Microsoft.JSInterop;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace ide.Components.Engine
{
    public class Initializer : IDisposable
    {
        Dictionary<string, dynamic> _Funcs = new();
        IJSRuntime JSRuntime;
        IWorkerBackgroundService<Core>? service;
        //NetworkStreamに変更の検討
        public async Task Initialize(IWorkerFactory workerFactory, LinkedList<(string, string, dynamic)> funcs, IJSInProcessRuntime JSRuntime, string script, List<Assembly>? assemblies = default)
        {
            if (assemblies == null)
            {
                assemblies = new List<Assembly>();
            }
            foreach (var item in funcs)
            {
                _Funcs.Add(item.Item1, item.Item3);
            }
            var funcsRelay = funcs.ToArray(); 
            this.JSRuntime = JSRuntime;
            // Create worker.
            var worker = await workerFactory.CreateAsync();
            // Create service reference. For most scenarios, it's safe (and best) to keep this 
            // reference around somewhere to avoid the startup cost.
            this.service = await worker.CreateBackgroundServiceAsync<Core>();
            await service.RunAsync(s => s.Initializer(funcsRelay, JSRuntime, assemblies));
            var result = await service.RunAsync(s => s.Ignition(script, false));
        }
        public async Task Bowling()
        {
            while (true)
            {
                var funcOptions = await this.JSRuntime.InvokeAsync<string>("SessionStorageWrite", new string[] { Settings.FunctionBridgeName });
                var funcOrders = funcOptions.Substring("DoAction".Length).Split(',');
                if (funcOrders.Length == 3 && _Funcs[funcOrders[0]] != null)
                {
                    var noWaitValueTask = this.JSRuntime.InvokeVoidAsync("SessionStorageRemove", new string[] { Settings.FunctionBridgeName });
                    var noWaitTask = DoTask(
                        funcOrders[0], 
                        funcOrders[1], 
                        funcOrders[2].TrimStart('"').TrimEnd('"').Split("\"\a\""));//Del double top and last quotation
                }
                await Task.Delay(1000);
            }
        }

        public async Task DoTask(string id, string name, object[] args)
        {
            string fomattee = String.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                if (_Funcs[name].GetMethodInfo().ReturnType.FullName.IndexOf("Task") != -1)
                {
                    var result = await _Funcs[name]();
                    if (Settings.takingRisk)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(ms, result);
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(Type.GetType(result));
                        serializer.Serialize(ms, result);
                    }
                }
                else
                {
                    var result = await Task.Run(() => _Funcs[name]());
                    if (Settings.takingRisk)
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(ms, result);
                    }
                    else
                    {
                        XmlSerializer serializer = new XmlSerializer(Type.GetType(result));
                        serializer.Serialize(ms, result);
                    }
                }
                byte[] bytes = new byte[(int)ms.Length];
                await ms.ReadAsync(bytes, 0, (int)ms.Length);
                fomattee = Settings.defaultEncoding.GetString(bytes);
                await this.JSRuntime.InvokeVoidAsync("SessionStorageWrite", new string[] { id, fomattee });
            }
        }
        public void Dispose()
        {

        }
    }
}