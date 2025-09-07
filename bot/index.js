import { Client, Events, GatewayIntentBits, REST, Routes } from 'discord.js';
import { CLIENT_ID, GUILD_ID, TOKEN } from './creds.js';

const commands = [
  {
    name: 'ping',
    description: 'Replies with Pong!',
  },

  {
    name: 'helloapi',
    description: 'Replies with hello from the API',
  },
];

const rest = new REST({ version: '10' }).setToken(TOKEN);

try {
  console.log('Started refreshing application (/) commands.');

  await rest.put(Routes.applicationGuildCommands(CLIENT_ID, GUILD_ID), { body: commands });

  console.log('Successfully reloaded application (/) commands.');
} catch (error) {
  console.error(error);
}

const client = new Client({ intents: [GatewayIntentBits.Guilds] });

client.on(Events.ClientReady, readyClient => {
  console.log(`Logged in as ${readyClient.user.tag}!`);
});

client.on(Events.InteractionCreate, async interaction => {
  if (!interaction.isChatInputCommand()) return;

  if (interaction.commandName === 'helloapi') {
    try {
      const response = await fetch("http://localhost:5272/api/hello");
      const data = await response.json();
      await interaction.reply(data.message);
    } catch (error) {
      console.error(error);
      await interaction.reply('Error fetching API');
    }
  }

  if (interaction.commandName === 'ping') {
    await interaction.reply('Pong!');
  }
});

client.login(TOKEN);
