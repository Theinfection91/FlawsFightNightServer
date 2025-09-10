import { SlashCommandBuilder } from 'discord.js';
import createSubcommand from './create.js';
// import other subcommands here

export default {
    data: new SlashCommandBuilder()
        .setName('tournament')
        .setDescription('Tournament related commands')
        .addSubcommand(createSubcommand.data),
    async execute(interaction) {
        const subcommand = interaction.options.getSubcommand();

        switch (subcommand) {
            case 'create':
                await createSubcommand.execute(interaction);
                break;
            // add other subcommands here
        }
    }
};
