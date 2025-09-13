import { SlashCommandBuilder } from 'discord.js';
import registerSubCommand from './register.js';
import deleteSubCommand from './delete.js';
// import other subcommands here

export default {
    data: new SlashCommandBuilder()
        .setName('team')
        .setDescription('Team related commands')
        .addSubcommand(registerSubCommand.data)
        .addSubcommand(deleteSubCommand.data),
    async execute(interaction) {
        const subcommand = interaction.options.getSubcommand();

        switch (subcommand) {
            case 'register':
                await registerSubCommand.execute(interaction);
                break;
            case 'delete':
                await deleteSubCommand.execute(interaction);
                break;
            // add other subcommands here
        }
    }
};
