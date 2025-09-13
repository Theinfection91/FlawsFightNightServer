import { SlashCommandSubcommandBuilder } from "discord.js";
import { apiClient } from "../../apiClient.js";

export default {
    data: new SlashCommandSubcommandBuilder()
        .setName("add-debug-admin")
        .setDescription("Add a user to the debug admin list")
        .addUserOption(option =>
            option.setName("user")
                .setDescription("The user to add")
                .setRequired(true)),
    async execute(interaction) {
        const user = interaction.options.getUser("user");

        const payload = {
            userId: user.id,
        };

        try {
            const data = await apiClient(`/settings/add-debug-admin`, {
                method: "PUT",
                body: payload,
            });

            await interaction.reply(`✅ ${data.message}`);
        } catch (error) {
            console.error("Full error object:", error);
            await interaction.reply(`❌ Failed to add debug admin\nError message: ${error.message || error}`);
        }
    },
};