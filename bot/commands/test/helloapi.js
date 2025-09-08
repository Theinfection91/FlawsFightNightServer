import { SlashCommandBuilder } from "discord.js";

export default {
  data: new SlashCommandBuilder()
    .setName("helloapi")
    .setDescription("Replies with a greeting from the API."),
  
  async execute(interaction) {
    try {
      const response = await fetch("http://localhost:5000/api/hello");
      const data = await response.json();
      await interaction.reply(data.message);
    } catch (error) {
      console.error(error);
      await interaction.reply("Error fetching API");
    }
  },
};
