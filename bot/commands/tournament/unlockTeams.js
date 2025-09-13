import { SlashCommandSubcommandBuilder } from "discord.js";
import { apiClient } from "../../apiClient.js";

export default {
    data: new SlashCommandSubcommandBuilder()
        .setName("unlock-teams")
        .setDescription("Unlock teams, allowing changes to a tournament before starting.")
        .addStringOption((option) =>
            option
        .setName("tournamentid")
        .setDescription("The ID of the tournament")
        .setRequired(true)
        ),
    async execute(interaction) {
        const tournamentId = interaction.options.getString("tournamentid");

        const payload = {
            tournamentId,
        };

        try {
            const data = await apiClient(`/tournaments/unlock-teams`, {
                method: "PATCH",
                body: payload,
            });

            await interaction.reply(`✅ ${data.message}`);
        } catch (error) {
            console.error("Full error object:", error);
            await interaction.reply(`❌ Failed to unlock teams\nError message: ${error.message || error}`);
        }
    },
};