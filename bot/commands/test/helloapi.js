import { SlashCommandBuilder } from "discord.js";
import { apiClient } from "../../appClient.js";

export default {
  data: new SlashCommandBuilder()
    .setName("helloapi")
    .setDescription("Replies with a greeting from the API."),
  
  async execute(interaction) {
    try {
      const data = await apiClient("/hello");
      await interaction.reply(data.message);
    } catch (error) {
      console.error(error);
      await interaction.reply("Error fetching API");
    }
  },
};
