import { SlashCommandSubcommandBuilder } from 'discord.js';
import { apiClient } from '../../apiClient.js';

export default {
    data: new SlashCommandSubcommandBuilder()
        .setName('create')
        .setDescription('Creates a new tournament.')
        .addStringOption(option =>
            option
                .setName('name')
                .setDescription('The name of the tournament')
                .setRequired(true)
        )
        .addStringOption(option =>
            option
                .setName('type')
                .setDescription('The type of the tournament')
                .setRequired(true)
                .addChoices(
                    { name: 'Ladder', value: 'ladder' },
                    { name: 'Round Robin', value: 'RoundRobin' },
                    { name: 'Single Elimination', value: 'single_elimination' },
                    { name: 'Double Elimination', value: 'double_elimination' }
                )
        )
        .addIntegerOption(option =>
            option
                .setName('team_size')
                .setDescription('Number of players per team')
                .setRequired(true)
                .setMinValue(1)
        ),

    async execute(interaction) {
        const tournamentName = interaction.options.getString('name');
        const tournamentType = interaction.options.getString('type');
        const teamSize = interaction.options.getInteger('team_size');
        const guildId = interaction.guildId;

        if (!guildId) {
            await interaction.reply('❌ This command can only be used in a server.');
            return;
        }

        const payload = {
            name: tournamentName,
            type: tournamentType,
            team_size: teamSize,
            guild_id: guildId
        };

        try {
            const data = await apiClient('/tournaments/create', {
                method: 'POST',
                body: payload
            });

            await interaction.reply(
                `✅ ${data.message}\nTournament ID: **${data.tournamentId}**\nName: **${data.tournamentName}**\nType: **${data.tournamentType}**\nFormat: **${data.teamSizeFormat}**`
            );
            await interaction.followUp('Tournament created successfully!');
        } catch (error) {
            console.error('Full error object:', error);  // <-- log everything
            await interaction.reply(
                `❌ Failed to create tournament\nError message: ${error.message || error}\nStatus: ${error.status || 'unknown'}\nBody: ${JSON.stringify(error.body || error)}`
            );
        }
    },
};