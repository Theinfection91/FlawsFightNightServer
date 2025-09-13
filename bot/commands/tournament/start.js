import { SlashCommandSubcommandBuilder } from "discord.js";
import { apiClient } from "../../apiClient.js";

export default {
    data: new SlashCommandSubcommandBuilder()
        .setName("start")
        .setDescription("Start a tournament")
        .addStringOption(option =>
            option.setName("tournamentId")
                .setDescription("The ID of the tournament to start")
                .setRequired(true)),
    async execute(interaction) {
        const tournamentId = interaction.options.getString("tournamentId");
        const guildId = interaction.guildId;

        if (!guildId) {
            await interaction.reply('❌ This command can only be used in a server.');
            return;
        }

        const payload = {
            tournamentId,
            guildId
        };

        try {
            const data = await apiClient(`/tournaments/start`, {
                method: "PATCH",
                body: payload,
            });

            await interaction.reply(`✅ ${data.message}`);
        } catch (error) {
            console.error("Full error object:", error);
            await interaction.reply(`❌ Failed to start tournament\nError message: ${error.message || error}`);
        }
    },
};