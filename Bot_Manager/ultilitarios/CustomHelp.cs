using System;
using System.Collections.Generic;
using System.Text;
using Bot_Manager.ComandosTexto;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;

namespace Bot_Manager.ultilitarios
{
    internal class CustomHelp : BaseHelpFormatter
    {

        protected StringBuilder _strBuilder;

        public CustomHelp(CommandContext ctx) : base(ctx)
        {
            _strBuilder = new StringBuilder();
        }



        public override BaseHelpFormatter WithCommand(Command command)
        {
            // _embed.AddField(command.Name, command.Description);            
            _strBuilder.AppendLine($"{command.Name} - {command.Description}");



            return this;
        }



        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> cmds)
        {
            foreach (var cmd in cmds)
            {
                // _embed.AddField(cmd.Name, cmd.Description);            
                _strBuilder.AppendLine($"{cmd.Name} - {cmd.Description}");
            }

            return this;
        }



        public override CommandHelpMessage Build()
        {
            return new CommandHelpMessage(embed: Textos.AjudaPadrao());
        }

    }
}
