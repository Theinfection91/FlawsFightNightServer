import { SlashCommandBuilder } from 'discord.js';
import { apiClient } from '../../apiClient.js';

export default {
    data: new SlashCommandBuilder()
        .setName('create_tournament')
        .setDescription('Creates a new tournament.')
        .addStringOption(option =>
            option
                .setName('name')
                .setDescription('The name of the tournament')
                .setRequired(true)
        )
        .addStringOption(option =>
            option
                .setName('id')
                .setDescription('Custom id for testing purposes')
                .setRequired(true)
        ),
        // .addStringOption(option =>
        //     option
        //         .setName('type')
        //         .setDescription('The type of the tournament')
        //         .setRequired(true)
        //         .addChoices(
        //             { name: 'Ladder', value: 'ladder' },
        //             { name: 'Round Robin', value: 'round_robin' },
        //             { name: 'Single Elimination', value: 'single_elimination' },
        //             { name: 'Double Elimination', value: 'double_elimination' }
        //         )
        // )
        // .addIntegerOption(option =>
        //     option
        //         .setName('team_size')
        //         .setDescription('Number of players per team')
        //         .setRequired(true)
        //         .setMinValue(1)
        // ),

    async execute(interaction) {
        const tournamentName = interaction.options.getString('name');
        const tournamentId = interaction.options.getString('id');
        //const tournamentType = interaction.options.getString('type');
        //const teamSize = interaction.options.getInteger('team_size');

        try {
            const created = await apiClient('/tournaments/create', {
                method: 'POST',
                body: {
                    name: tournamentName,
                    id: tournamentId,
                    //type: tournamentType,
                    //team_size: teamSize
                },
            });
            await interaction.reply(`API response: ${JSON.stringify(created)}`);
            await interaction.followUp('Tournament created successfully!');
        } catch (error) {
            if (error.status === 409) {
                await interaction.reply('That tournament ID already exists!');
            } else {
                console.error(error);
                await interaction.reply('Failed to create tournament via API.');
            }
        }
    },
};