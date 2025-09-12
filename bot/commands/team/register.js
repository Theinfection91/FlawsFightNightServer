import { SlashCommandSubcommandBuilder } from 'discord.js';
import { apiClient } from '../../apiClient.js';

export default {
    data: new SlashCommandSubcommandBuilder()
        .setName('register')
        .setDescription('Registers a new team.')
        .addStringOption(option =>
            option.setName('name')
                .setDescription('The name of the team')
                .setRequired(true))
        .addStringOption(option =>
            option.setName('tournamentid')
                .setDescription('The ID of the tournament')
                .setRequired(true))
        // For now, command will accept up to 5 members
        .addUserOption(option =>
            option.setName('member1')
                .setDescription('The first member of the team')
                .setRequired(true))
        .addUserOption(option =>
            option.setName('member2')
                .setDescription('The second member of the team')
                .setRequired(false))
        .addUserOption(option =>
            option.setName('member3')
                .setDescription('The third member of the team')
                .setRequired(false))
        .addUserOption(option =>
            option.setName('member4')
                .setDescription('The fourth member of the team')
                .setRequired(false))
        .addUserOption(option =>
            option.setName('member5')
                .setDescription('The fifth member of the team')
                .setRequired(false)),

    async execute(interaction) {
        const teamName = interaction.options.getString('name');
        const tournamentId = interaction.options.getString('tournamentid');

        // Collect members as dictionary { userId: username }
        const members = {};
        for (let i = 1; i <= 5; i++) {
            const user = interaction.options.getUser(`member${i}`);
            if (user) {
                members[user.id] = user.username; // use .username or ask for in-game name separately
            }
        }

        // Build request matching C# DTO
        const payload = {
            teamName,
            tournamentId,
            members
        };

        try {
            const data = await apiClient('/teams/register', {
                method: 'POST',
                body: payload, // <- plain object
            });

            await interaction.reply(
                `✅ ${data.message}\nTeam ID: **${data.teamId}**\nName: **${data.teamName}**\nMembers: ${Object.values(data.members).join(', ')}`
            );
        } catch (error) {
            console.error('Full error object:', error);  // <-- log everything
            await interaction.reply(
                `❌ Failed to register team\nError message: ${error.message || error}\nStatus: ${error.status || 'unknown'}\nBody: ${JSON.stringify(error.body || error)}`
            );
        }

    },
};
