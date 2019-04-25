﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceClient.Websocket.Models;
using BinanceClient.Websocket.Models.Args;
using BinanceClient.Websockets;
using Newtonsoft.Json;

namespace BinanceClient.Websocket
{
    public class DiffDepth : IWebsocketStream
    {
        Websockets.WebsocketClient _ws;
        public event EventHandler<DiffDepthArgs> OnDiffDepthReceived;

        public void ProcessRecievedMessage(string payload)
        {
            var handler = OnDiffDepthReceived;
            if (handler != null)
            {
                var msg = JsonConvert.DeserializeObject<DiffDepthMessage>(payload);
                var arg = new DiffDepthArgs { OrderBookUpdate = msg.Data };
                handler(this, arg);
            }
        }

        public DiffDepth(Websockets.WebsocketClient ws)
        {
            _ws = ws;
        }

        public void Subscribe(string symbol)
        {
            var msg = new { method = "subscribe", topic = "marketDiff", symbols = new[] { symbol } };
            _ws.Send(msg);
        }

        public void Unsubscribe(string symbol)
        {
            var msg = new { method = "unsubscribe", topic = "marketDiff", symbols = new[] { symbol } };
            _ws.Send(msg);
        }
    }

}
