# Robot message collection service

This is a windows server program, the porgram will automatically runnning on the robot server after the server starts up.

On each robot runs the "KRMSGNET", a service of KUKA robot, which sends the messages to a specified path via UDP telegram.

This service will collect the telegrams from robots plantwide, then store it into ACCESS database, so that there will be a central data collector for all robots.

Based on the data, problems like "system error", "brake test failure" and so on can be tracked and logged, how often and in which robots do they happen often, questions like this can be supported by collected data.

This tool helped me a lot with communication with my supplier.

In order to keep the performance of the database, data that older than 90 days will be deleted weekly, the configuration about this can be set in the app.config.

