import { SlashCommandBuilder } from "discord.js";
import addDebugAdmin from "./addDebugAdmin.js";

export default {
    data: new SlashCommandBuilder()
        .setName("settings")
        .setDescription("Manage server settings")
        .addSubcommand(addDebugAdmin.data),
    async execute(interaction) {
        const subcommand = interaction.options.getSubcommand();

        switch (subcommand) {
            case "add-debug-admin":
                await addDebugAdmin.execute(interaction);
                break;
            // Add more subcommands here as needed
        }
    },
};