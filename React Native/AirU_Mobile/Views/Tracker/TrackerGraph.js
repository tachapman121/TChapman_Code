import React, {Component} from 'react';
import {View, Button, TouchableHighlight, Image} from 'react-native';
import { LineChart, Grid } from 'react-native-svg-charts';
import * as shape from 'd3-shape';
import { Circle, G, Line, Rect, Text } from 'react-native-svg';

export default class TrackerGraph extends Component<Props>
{
    constructor(props) {
        super(props);
    }
    render() {
        const data = this.props.data;
        const selectedIndex = this.props.selectedIndex;
        const Tooltip = ({ x, y }) => (
            <G
                x={ x(selectedIndex) - (65 / 2) }
                key={ 'tooltip' }
            >
                <G y={ y(data[ selectedIndex ]) - 35 }>
                    <Rect
                        height={ 20 }
                        width={ 50 }
                        stroke={ 'blue' }
                        fill={ 'white' }
                        ry={ 10 }
                        rx={ 10 }
                    />
                    <Text
                        x={ 45 / 2 }
                        dy={ 10 }
                        alignmentBaseline={ 'middle' }
                        textAnchor={ 'middle' }
                        stroke={ 'rgb(0, 0, 255)' }
                    >
                        { `${data[selectedIndex]}` }
                    </Text>
                </G>
                <G x={ 50 / 2 }>
                    <Circle
                        cy={ y(data[ selectedIndex ]) }
                        r={ 6 }
                        stroke={ 'rgb(0, 0, 255)' }
                        strokeWidth={ 2 }
                        fill={ 'white' }
                    />
                </G>
            </G>
        )

        return (
            <LineChart
                style={{ height: 150, backgroundColor: "white" }}
                data={ data }
                svg={{
                    stroke: 'rgb(0, 0, 255)',
                    strokeWidth: 2,
                }}
                yMin={0}
                yMax={70}
                contentInset={{ top: 25, bottom: 20, left: 40, right: 25 }}
                curve={ shape.curveLinear }
            >
                <Grid/>
                <Tooltip/>
            </LineChart>
        )
    };
}
