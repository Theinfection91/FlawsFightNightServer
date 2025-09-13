import { SlashCommandSubcommandBuilder } from "discord.js";
import { apiClient } from "../../apiClient.js";

export default {
    data: new SlashCommandSubcommandBuilder()
        .setName("delete")
        .setDescription("Deletes a team.")
        .addStringOption(option =>
            option
                .setName("teamid")
                .setDescription("The ID of the team to delete")
                .setRequired(true)
        )
        .addStringOption(option =>
            option
                .setName("tournamentid")
                .setDescription("The ID of the tournament the team is in")
                .setRequired(true)
        ),
    async execute(interaction) {
        const teamId = interaction.options.getString("teamid");
        const tournamentId = interaction.options.getString("tournamentid");
        const guildId = interaction.guildId;

        if (!guildId) {
            await interaction.reply('❌ This command can only be used in a server.');
            return;
        }

        // Build request matching C# DTO
        const payload = {
            teamId,
            tournamentId,
            guildId
        };

        try {
            const data = await apiClient(`/teams/delete`, {
                method: "DELETE",
                body: payload,
            });

            await interaction.reply(`✅ ${data.message}\nTeam Name and ID#: ${data.teamName}(#${data.teamId}) has been deleted.`);
        } catch (error) {
            console.error("Full error object:", error);
            await interaction.reply(`❌ Failed to delete team\nError message: ${error.message || error}`);
        }
    },
};
