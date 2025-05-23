﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Text;
using wx.Infrastructure.LLM;

namespace wx.Api.Controllers;

public class TestController : WxController
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TestController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("chat")]
    public async Task<IActionResult> TestChat(CancellationTokenSource cts)
    {
        LlmConfig config = new LlmConfig();
        config.BaseUrl = "https://spark-api-open.xf-yun.com/v1";
        config.ApiSecret = "";
        config.ApiKey = "";
        LlmProviderFactory.Instance.Initialize(config);
        var llm = LlmProviderFactory.Instance.GetProvider("spark");

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["model"] = "lite";
        dic["stream"] = true;

        var str = await llm.ChatCompletionsAsync(new[] { new  LlmMessage(){ Role = "user", Content = "你是谁" } }, dic, cts.Token);
        return Ok(str);
    }

    [HttpGet("AI")]
    public async Task TestAI()
    {
        var url = "https://spark-api-open.xf-yun.com/v1/chat/completions";
        var client = _httpClientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "");
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["model"] = "lite";
        var list = new List<Dictionary<string, string>>();
        list.Add(new Dictionary<string, string>()
        {
            {"role" ,"system"},
            {"content" ,"你是一名文学专家专家"}
        });

        list.Add(new Dictionary<string, string>()
        {
            {"role" ,"user"},
            {"content" ,"帮我写一篇800字的描写故宫的文章"}
        });
        dic["messages"] = list;
        dic["stream"] = true;

        var data = JsonConvert.SerializeObject(dic);

        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");

        HttpContent httpContent = new StringContent(data, Encoding.UTF8, "application/json");
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = httpContent
        };
        using var resp = await client.SendAsync(
                 request,
                 HttpCompletionOption.ResponseHeadersRead,
                 HttpContext.RequestAborted
             );
        resp.EnsureSuccessStatusCode();

        using var stream = await resp.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var eventData = new System.Text.StringBuilder();
        string line;
        var cts = HttpContext.RequestAborted;
        while (!cts.IsCancellationRequested && (line = await reader.ReadLineAsync()) != null)
        {
            eventData.AppendLine(line);
            if (string.IsNullOrEmpty(line))
            {
                var eventBytes = System.Text.Encoding.UTF8.GetBytes(eventData.ToString());
                await Response.Body.WriteAsync(eventBytes, 0, eventBytes.Length, cts);
                await Response.Body.FlushAsync(cts);
                eventData.Clear();
            }
        }
        Response.Body.Close();
    }
}
