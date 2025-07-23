
@section Scripts {
    <script>

        //  Generate time slots (2 hours before to 2.5 hours after current time)
        function generateTimeIntervals() {
            const current = new Date();
        current.setSeconds(0, 0);
        const roundedStart = new Date(current.getFullYear(), current.getMonth(), current.getDate(), current.getHours(), current.getMinutes() < 30 ? 0 : 30);
        roundedStart.setMinutes(roundedStart.getMinutes() - 120); // Back by 2 hours

        const intervals = [];
        for (let i = 0; i < 9; i++) {
                const startTime = new Date(roundedStart.getTime() + i * 30 * 60000);
        const endTime = new Date(startTime.getTime() + 30 * 60000);
        intervals.push({startTime, endTime});
            }

        return intervals;
        }

        // Render the schedule table based on fetched JSON data
        function renderBroadcastGrid(broadcastItems) {
            const intervals = generateTimeIntervals();
        const now = new Date();
        now.setSeconds(0, 0);
            const activeSlotIndex = intervals.findIndex(interval => now >= interval.startTime && now < interval.endTime);

        let output = `<table class="table table-bordered text-center"><thead><tr><th>Program Title</th>`;
            intervals.forEach((interval, idx) => {
                const highlight = idx === activeSlotIndex ? 'table-primary' : '';
            output += `<th class="${highlight}">
                ${interval.startTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}<br>to<br>
                    ${interval.endTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                </th>`;
            });
                    output += `</tr></thead><tbody>`;

            broadcastItems.forEach(program => {
                    output += `<tr><td class="text-start fw-semibold">${program.title}</td>`;
                intervals.forEach((interval, idx) => {
                    const progStart = new Date(program.startTime);
                const progEnd = new Date(program.endTime);
                const overlaps = progStart < interval.endTime && progEnd > interval.startTime;
                const isNow = idx === activeSlotIndex;

                let cellStyle = '';
                if (overlaps) cellStyle += 'bg-success text-white ';
                if (isNow) cellStyle += 'table-primary';

                output += `<td class="${cellStyle.trim()}">${overlaps ? program.description : ''}</td>`;
                });
                output += `</tr>`;
            });

            output += `</tbody></table>`;
        document.getElementById("broadcastTableContainer").innerHTML = output;
        }

        // ✅ Load schedule from server (broadcasts.json)
        function fetchAndDisplaySchedule() {
            const url = `/Json/Broadcast.json?_ts=${Date.now()}`; // Avoid cache
        fetch(url)
                .then(response => {
                    if (!response.ok) throw new Error("Unable to fetch broadcast data.");
        return response.json();
                })
                .then(data => renderBroadcastGrid(data))
                .catch(error => {
            console.error("Schedule Error:", error.message);
        document.getElementById("broadcastTableContainer").innerHTML = `<div class="alert alert-danger">Failed to load schedule. ${error.message}</div>`;
                });
        }

        fetchAndDisplaySchedule();
        setInterval(fetchAndDisplaySchedule, 30 * 60 * 1000); // Refresh every 30 mins

    </script>
}
