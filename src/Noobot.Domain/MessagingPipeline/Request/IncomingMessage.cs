﻿using System;
using System.Linq;
using Noobot.Domain.MessagingPipeline.Response;

namespace Noobot.Domain.MessagingPipeline.Request
{
    public class IncomingMessage
    {
        public int MessageId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public string Channel { get; set; }
        public string UserChannel { get; set; }
        public string BotName { get; set; }
        public string BotId { get; set; }

        private string _formattedText;
        public string FormatTextTargettedAtBot()
        {
            if (string.IsNullOrEmpty(_formattedText))
            {
                string[] myNames =
                {
                    BotName + ":",
                    BotName,
                    string.Format("<@{0}>:", BotId),
                    string.Format("<@{0}>", BotId),
                    string.Format("@{0}:", BotName),
                    string.Format("@{0}", BotName),
                };

                string handle = myNames.FirstOrDefault(x => Text.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));
                if (!string.IsNullOrEmpty(handle))
                {
                    _formattedText = Text.Substring(handle.Length).Trim();
                }
            }

            return _formattedText ?? string.Empty;
        }


        public ResponseMessage ReplyToChannel(string format, params object[] values)
        {
            string text = string.Format(format, values);
            return ReplyToChannel(text);
        }

        public ResponseMessage ReplyToChannel(string text)
        {
            return ResponseMessage.ChannelMessage(Channel, text);
        }

        public ResponseMessage ReplyDirectlyToUser(string format, params object[] values)
        {
            string text = string.Format(format, values);
            return ReplyDirectlyToUser(text);
        }

        public ResponseMessage ReplyDirectlyToUser(string text)
        {
            return ResponseMessage.DirectUserMessage(UserChannel, UserId, text);
        }
    }
}