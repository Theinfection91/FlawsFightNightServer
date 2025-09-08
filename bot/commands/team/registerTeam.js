import { SlashCommandBuilder } from 'discord.js';

export default {
    data: new SlashCommandBuilder()
        .setName('registerteam')
        .setDescription('Registers a new team.')
        .addStringOption(option =>
            option.setName('name')
                .setDescription('The name of the team')
                .setRequired(true)),
                
    async execute(interaction) {
        const teamName = interaction.options.getString('name');
        await interaction.reply(`Team ${teamName} has been registered!`);
    },
};
