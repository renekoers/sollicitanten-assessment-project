import React, { useState, useEffect } from "react";
import { BarChart, Bar, XAxis, YAxis, Cell, CartesianGrid } from "recharts";
import "../../css/Statistics.css";

export function Statistics(props){
    const [id, setId] = useState(props.id);
    const [components, setComponents] = useState([]);
    useEffect(() => {
        setId(props.id);
    },[props.id])

    useEffect(() => {
        async function getData(){
            let unsolved = null;
            let dataEveryone = null;
            await fetchStatistics("unsolved")
            .then(data => unsolved=data)
            await fetchStatistics("tally")
            .then(data => dataEveryone=data)
            await fetchStatistics("candidate?id=" + id)
            .then(dataCandidate => {
                var newComponents = []
                for (const [levelNumber, dictionaryEveryone] of Object.entries(
                    dataEveryone
                )) {
                newComponents.push(getLevelComponents(levelNumber, dictionaryEveryone, dataCandidate[levelNumber], unsolved[levelNumber]));
                }
                setComponents(newComponents);
            })
        }

        function fetchStatistics(nameApi){
            return new Promise(function(resolve, reject){
                if(id===null){
                    reject(400);
                }
                fetch("api/statistics/" + nameApi, {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: localStorage.getItem("token")
                    }
                })
                .then(status)
                .then(data => resolve(data))
                .catch(error => reject(error))
            })
        }

        function status(response){
            return new Promise(function(resolve, reject){
                if(response.status === 200){
                    resolve(response.json())
                } else {
                    reject(response.status)
                }
            })
        }

        function getLevelComponents(levelNumber, levelStatisticsEveryone, levelStatisticsCandidate, amountUnsolved){
            var statisticComponents = [];
            for(const[nameData, results] of Object.entries(levelStatisticsEveryone)){
                var singleStatisticCandidate = levelStatisticsCandidate!==undefined ? levelStatisticsCandidate[nameData] : "unsolved"
                statisticComponents.push(
                    <LevelBarChart
                        className="level-chart"
                        key={"D" + levelNumber}
                        name={nameData}
                        tallyData={results}
                        candidateData={
                            singleStatisticCandidate
                        }
                        unsolved = {amountUnsolved}
                    />
                )
            }
            return(
                <div
                    className="level-chart-container"
                    key={"C" + levelNumber}
                >
                    <label
                        className="level-chart-label"
                        key={"L" + levelNumber}
                    >
                        Level {levelNumber}
                    </label>
                    <div className="stats-container">
                        {statisticComponents.map((component, index) => (
                            <div key={levelNumber + "-" + index} className="chart">{component}</div>
                        ))}
                    </div>
                </div>
            )
        }
        setComponents([])
        getData()
    },[id])



    return (
        <div>
            {components.map((component, index) => (
                <div key={index}>{component}</div>
            ))}
        </div>
    );
}
const AxisLabel = ({ axisType, x, y, width, height, stroke, children }) => {
	const isVert = axisType === "yAxis";
	const cx = isVert ? x : x + width / 2;
	const cy = isVert ? height / 2 + y : y + height + 10;
	const rot = isVert ? `270 ${cx} ${cy}` : 0;
	return (
		<text
			x={cx}
			y={cy}
			transform={`rotate(${rot})`}
			textAnchor="middle"
			stroke={stroke}
		>
			{children}
		</text>
	);
};
function LevelBarChart(props){
    const [dataTally, setDataTally] = useState([]);
    useEffect(() => {
        const _dataTally = [];
		for (const [_lines, _candidates] of Object.entries(props.tallyData)) {
			_dataTally.push({
				lines: _lines,
				candidates: Number.parseInt(_candidates)
			});
        }
        _dataTally.push({lines: "unsolved", candidates: props.unsolved})
        setDataTally(_dataTally);
    },[props.tallyData])

    return (
        <BarChart
            width={300}
            height={200}
            data={dataTally}
            margin={50}
        >
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis
                dataKey="lines"
                label={{ value: props.name, position: "insideBottom" }}
                height={40}
            />
            <YAxis
                label={
                    <AxisLabel
                        axisType="yAxis"
                        x={25}
                        y={-75}
                        width={0}
                        height={300}
                    >
                        Kandidaten
                    </AxisLabel>
                }
                allowDecimals={false}
            />
            <Bar dataKey="candidates">
                {dataTally.map((entry, index) => {
                    return (
                        <Cell
                            key={`cell-${index}`}
                            fill={
                                entry.lines === "" + props.candidateData
                                    ? "#00AA00"
                                    : "#8CA183"
                            }
                        />
                    );
                })}
            </Bar>
        </BarChart>
    );
}