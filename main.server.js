export default class NotABotStats {
	static running = false;

	static init() {
		NotABotStats.update();

		// Update every day at 6 PM utc
		const nextUpdate = new Date();
		nextUpdate.setUTCHours(6,5,0,0);
		clearTimeout(NotABotStats.timeout);
		NotABotStats.timeout = setTimeout(() => {
			NotABotStats.update();

			clearInterval(NotABotStats.interval);
			NotABotStats.interval = setInterval(NotABotStats.update, 24 * 60 * 60 * 1000);
		}, Date.now() - nextUpdate.getTime());
	}

	static close() {
		clearInterval(NotABotStats.interval);
	}

	static async getLatestStatDate(serverId) {
		const res = await Database.execQuery('SELECT MAX(date) as date FROM public.notabot_stats_server WHERE id = $1', [serverId])

		if(!res.rows || res.rows.length === 0 || res.rows[0].date === null) {
			return new Date(2017, 1, 1);
		}

		const date = new Date(res.rows[0].date);
		date.setDate(date.getDate() + 1); 

		return date;
	}

	static async update() {
		if(NotABotStats.running) {
			return;
		}
		NotABotStats.running = true;

		log('Updating Not a bot stats ...', 'info');

		const DAY_INTERVAL = (24*60*60*1000);
		const maxDate = Date.now() - DAY_INTERVAL;

		for(const server of config.servers) {
			const latestStat = await NotABotStats.getLatestStatDate(server.id);

			let authHeaders = {};
			if(server.authToken) {
				authHeaders = {
					'Authorization': 'Bearer ' + server.authToken,
				};
			}

			for(let i = latestStat.getTime(); i < maxDate; i += DAY_INTERVAL) {
				const date = new Date(i);
				const formattedDate = date.getFullYear() + '-' + (date.getMonth() + 1).toString().padStart(2, '0') + '-' + date.getDate().toString().padStart(2, '0'); 

				log(`Updating Not a bot stats ... Server "${server.id}", Date "${formattedDate}"`, 'info');

				const res = await fetch(`${server.url}/stats/server/${server.id}/day/${formattedDate}`, { headers: {
					'user-agent': `L.U.M.E.N. stats gatherer - By Elanis - https://github.com/L-U-M-E-N/lumen-module-notabot-stats`,
					...authHeaders,
				}});

				if(!res.ok) {
					continue;
				}

				const stats = await res.json();
				const statsDate = new Date(stats.Date * 1000); 

				// Push server stats
				try {
					await Database.execQuery(
						`INSERT INTO notabot_stats_server
							(id, name, member_left, member_joined, messagecount, reactionadded, reactionremoved, membercount, date)
							VALUES 
							($1, $2  , $3         , $4           , $5          , $6           , $7             , $8         , $9)
						`, [server.id, server.name, stats.MemberLeft, stats.MemberJoined, stats.MessageCount, stats.ReactionAdded, stats.ReactionRemoved, stats.MemberCount || 0, statsDate]
					);
				} catch(e) {
					log(e, 'error');
					break;
				}

				// Push server reactions stats
				for(const reactionName in stats.Reactions) {
					try {
						await Database.execQuery(
							`INSERT INTO notabot_stats_server_reactions
								(id, date, reaction, amount)
								VALUES 
								($1, $2  , $3      , $4)
							`, [server.id, statsDate, reactionName, stats.Reactions[reactionName].ReactionCount]
						);
					} catch(e) {
						log(e, 'error');
						break;
					}
				}

				// Push server channels stats
				for(const channelName in stats.Channels) {
					try {
						await Database.execQuery(
							`INSERT INTO notabot_stats_server_channels
								(id, date, channel, messagecount, reactioncount)
								VALUES 
								($1, $2  , $3      , $4         , $5)
							`, [server.id, statsDate, channelName, stats.Channels[channelName].MessageCount, stats.Channels[channelName].ReactionCount]
						);
					} catch(e) {
						log(e, 'error');
						break;
					}
				}

				// Push server users stats
				for(const userName in stats.Users) {
					try {
						await Database.execQuery(
							`INSERT INTO notabot_stats_server_users
								(id, date, userid, messagecount, reactioncount)
								VALUES 
								($1, $2  , $3      , $4         , $5)
							`, [server.id, statsDate, userName, stats.Users[userName].MessageCount, stats.Users[userName].ReactionCount]
						);
					} catch(e) {
						log(e, 'error');
						break;
					}
				}
			} 
		}

		log('Updating Not a bot stats ... Done !', 'info');

		
		SteamSoldWishlist.running = false;
	}
}

NotABotStats.init();