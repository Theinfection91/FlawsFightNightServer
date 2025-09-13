import { SlashCommandSubcommandBuilder } from "discord.js";
import { apiClient } from "../../apiClient.js";

export default {
    data: new SlashCommandSubcommandBuilder()
        .setName("lock-teams")
        .setDescription("Lock teams, allowing a tournament to be started.")
        .addStringOption((option) =>
            option
        .setName("tournamentid")
        .setDescription("The ID of the tournament")
        .setRequired(true)
        ),

    async execute(interaction) {
        const tournamentId = interaction.options.getString("tournamentid");
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
            const data = await apiClient(`/tournaments/lock-teams`, {
                method: "PATCH",
                body: payload,
            });

            await interaction.reply(`✅ ${data.message}`);
        } catch (error) {
            console.error("Full error object:", error);
            await interaction.reply(`❌ Failed to lock teams\nError message: ${error.message || error}`);
        }
    },
};