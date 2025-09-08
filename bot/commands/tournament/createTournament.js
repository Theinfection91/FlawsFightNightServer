import { SlashCommandBuilder } from 'discord.js';

export default {
    data: new SlashCommandBuilder()
        .setName('createtournament')
        .setDescription('Creates a new tournament.')
        .addStringOption(option =>
            option.setName('name')
                .setDescription('The name of the tournament')
                .setRequired(true)),
    async execute(interaction) {
        const tournamentName = interaction.options.getString('name');
        await interaction.reply(`Tournament ${tournamentName} has been created!`);
    },
};
