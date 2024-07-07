using System;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace Arkcast
{
    [ApiVersion(2, 1)]
    public class Arkcast : TerrariaPlugin
    {
        public override string Name => "Arkcast";
        public override string Author => "Verza";
        public override string Description => "A customizable announcement plugin for Arkheim";
        public override Version Version => new Version(1, 1);

        public Arkcast(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command("Arkheim.announce.use", Announce, "announce")
            {
                HelpText = "Usage: /announce c/hexcode:message, c/hexcode:message ... or /announce help"
            });
        }

        private void Announce(CommandArgs args)
        {
            if (args.Parameters.Count == 0 || args.Parameters[0].ToLower() == "help")
            {
                args.Player.SendInfoMessage("[c/f2c59f:ARKCAST] [c/c2d5e3:Is a customizable broadcast plugin for] [c/b0e0e6:ARKREALM]");
                args.Player.SendInfoMessage("Usage: /announce c/hexcode:message, c/hexcode:message ...");
                args.Player.SendInfoMessage("Example: /announce c/ff0000:This is a red message, c/00ff00:This is a green message");
                args.Player.SendInfoMessage("Result: [c/ff0000:This is a red message] [c/00ff00:This is a green message]");
                return;
            }

            string[] messageParts = string.Join(" ", args.Parameters).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder messageBuilder = new StringBuilder();

            foreach (var part in messageParts)
            {
                var trimmedPart = part.Trim();
                if (!trimmedPart.StartsWith("c/") || !trimmedPart.Contains(":"))
                {
                    args.Player.SendErrorMessage("Invalid format. Use /announce help for more information.");
                    return;
                }

                var splitIndex = trimmedPart.IndexOf(':');
                var colorCode = trimmedPart.Substring(2, splitIndex - 2);
                var message = trimmedPart.Substring(splitIndex + 1);

                messageBuilder.Append($"[c/{colorCode}:{message}] ");
            }

            string finalMessage = messageBuilder.ToString().Trim();
            TShock.Utils.Broadcast(finalMessage, Microsoft.Xna.Framework.Color.White);
        }
    }
}

