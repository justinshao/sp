using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Core.Enums
{
    public enum LEDMoveMode
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Unkonw = -1,
        /// <summary>
        /// 立刻显示
        /// </summary>
        [Description("立刻显示")]
        RightNow = 0,
        /// <summary>
        /// 从右向左
        /// </summary>
        [Description("从右向左")]
        RightToLeft = 1,
        /// <summary>
        /// 从左向右
        /// </summary>
        [Description("从左向右")]
        LeftToRight = 2,
        /// <summary>
        /// 从上到下
        /// </summary>
        [Description("从上到下")]
        TopToBottom = 3,
        /// <summary>
        /// 从下到上
        /// </summary>
        [Description("从下到上")]
        BottomToTop = 4,
        /// <summary>
        /// 向下拉窗
        /// </summary>
        [Description("向下拉窗")]
        BottomSashWindow = 5,
        /// <summary>
        /// 向上拉窗
        /// </summary>
        [Description("向上拉窗")]
        TopSashWindow = 6,
        /// <summary>
        /// 向左拉窗
        /// </summary>
        [Description("向左拉窗")]
        LeftSashWindow = 7,
        /// <summary>
        /// 向右拉窗
        /// </summary>
        [Description("向右拉窗")]
        RightSashWindow = 8,
        /// <summary>
        /// 反亮显示
        /// </summary>
        [Description("反亮显示")]
        ReverseLight = 9,

        /// <summary>
        /// 无操作
        /// </summary>
        [Description("无操作")]
        NoOps = 10,
        /// <summary>
        /// 中速左移动
        /// </summary>
        [Description("中速左移动")]
        MinSpeedLeft = 11,
        /// <summary>
        /// 中速右移动
        /// </summary>
        [Description("中速右移动")]
        MinSpeedRight = 12,
        /// <summary>
        /// 逐字展开
        /// </summary>
        [Description("逐字展开")]
        Spread = 13,
        /// <summary>
        /// 向左开栅
        /// </summary>
        [Description("向左开栅")]
        ToLeftSpread = 14,
        /// <summary>
        /// 向左开栅
        /// </summary>
        [Description("向右开栅")]
        ToRightSpread = 15,
        /// <summary>
        /// 雪花显示
        /// </summary>
        [Description("雪花显示")]
        SnowFlake = 16,
        /// <summary>
        /// 隔行显示
        /// </summary>
        [Description("隔行显示")]
        InterlacedRow = 17,
        /// <summary>
        /// 隔列显示
        /// </summary>
        [Description("隔列显示")]
        InterlacedColumn = 18,
        /// <summary>
        /// 慢速左移
        /// </summary>
        [Description("慢速左移")]
        SlowLeft = 19,
        /// <summary>
        /// 慢速右移
        /// </summary>
        [Description("慢速右移")]
        SlowRight = 20,
        /// <summary>
        /// 连续左移动
        /// </summary>
        [Description("连续左移")]
        SeriesLeft = 21,
        /// <summary>
        /// 中速连续左移
        /// </summary>
        [Description("中速连续左移")]
        MinSpeedSeriesLeft = 22,
        /// <summary>
        /// 慢速连续左移
        /// </summary>
        [Description("慢速连续左移")]
        SlowSpeedSeriesLeft = 23,
    }
}
