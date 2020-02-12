import React, { useState, useEffect } from "react";
import { BarChart, Bar, XAxis, YAxis, Cell, CartesianGrid } from "recharts";
import "../../css/Statistics.css";

export function StatisticsLessRequests(props){
    const [id, setId] = useState(props.id);
    const [components, setComponents] = useState([]);
    useEffect(() => {
        setId(props.id);
    },[props.id])

    useEffect(() => {
        function fetchStatistics(nameApi){
            return new Promise(function(resolve, reject){
                if(id===null){
                    reject(400);
                }
                fetch("api/statistics/" + nameApi + "?id=" + id, {
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
        function getComponent(nameData, dataTally, dataCurrent){
            var newComponent = [];
            if (dataTally) {
                for (const [levelNumber, results] of Object.entries(
                    dataTally
                )) {
                    newComponent.push(
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
                            <LevelBarChart
                                className="level-chart"
                                key={"D" + levelNumber}
                                name={nameData}
                                tallyData={results}
                                candidateData={
                                    dataCurrent[levelNumber]
                                }
                            />
                        </div>
                    );
                }
                return newComponent;
            }
        }
        async function getData(){
            let dataTally = null;
            await fetchStatistics("tally")
            .then(data => dataTally=data)
            await fetchStatistics("candidate")
            .then(data => {
                var newComponents = []
                for (const [nameChart, dictionaryTally] of Object.entries(
                    dataTally
                )) {
                newComponents.push(getComponent(nameChart, dictionaryTally, data[nameChart]));
                }
                setComponents(newComponents);
            })
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
    const [nameChart, setNameChart] = useState(props.name)
    useEffect(() => {
        const _dataTally = [];
		for (const [_lines, _candidates] of Object.entries(props.tallyData)) {
			_dataTally.push({
				lines: Number.parseInt(_lines),
				candidates: Number.parseInt(_candidates)
			});
        }
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
                label={{ value: nameChart, position: "insideBottom" }}
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
                                entry.lines === props.candidateData
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