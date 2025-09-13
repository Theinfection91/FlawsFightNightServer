import { SlashCommandBuilder } from 'discord.js';
import createSubcommand from './create.js';
import lockTeamsSubcommand from './lockTeams.js';
import unlockTeamsSubcommand from './unlockTeams.js';

// import other subcommands here

export default {
    data: new SlashCommandBuilder()
        .setName('tournament')
        .setDescription('Tournament related commands')
        .addSubcommand(createSubcommand.data)
        .addSubcommand(lockTeamsSubcommand.data)
        .addSubcommand(unlockTeamsSubcommand.data),
    async execute(interaction) {
        const subcommand = interaction.options.getSubcommand();

        switch (subcommand) {
            case 'create':
                await createSubcommand.execute(interaction);
                break;
            case 'lock-teams':
                await lockTeamsSubcommand.execute(interaction);
                break;
            case 'unlock-teams':
                await unlockTeamsSubcommand.execute(interaction);
                break;
        }
    }
};
