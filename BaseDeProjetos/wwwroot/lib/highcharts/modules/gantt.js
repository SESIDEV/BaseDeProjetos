/*
 Highcharts Gantt JS v8.1.2 (2020-06-16)

 Gantt series

 (c) 2016-2019 Lars A. V. Cabrera

 License: www.highcharts.com/license
*/
(function (c) { "object" === typeof module && module.exports ? (c["default"] = c, module.exports = c) : "function" === typeof define && define.amd ? define("highcharts/modules/gantt", ["highcharts"], function (J) { c(J); c.Highcharts = J; return c }) : c("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (c) {
    function J(c, l, u, z) { c.hasOwnProperty(l) || (c[l] = z.apply(null, u)) } c = c ? c._modules : {}; J(c, "parts-gantt/Tree.js", [c["parts/Utilities.js"]], function (c) {
        var l = c.extend, u = c.isNumber, z = c.pick, x = function (l, y) {
            var k = l.reduce(function (b,
                g) { var m = z(g.parent, ""); "undefined" === typeof b[m] && (b[m] = []); b[m].push(g); return b }, {}); Object.keys(k).forEach(function (b, g) { var m = k[b]; "" !== b && -1 === y.indexOf(b) && (m.forEach(function (b) { g[""].push(b) }), delete g[b]) }); return k
        }, A = function (c, y, k, b, g, m) {
            var t = 0, H = 0, I = m && m.after, E = m && m.before; y = { data: b, depth: k - 1, id: c, level: k, parent: y }; var p, a; "function" === typeof E && E(y, m); E = (g[c] || []).map(function (w) {
                var e = A(w.id, c, k + 1, w, g, m), C = w.start; w = !0 === w.milestone ? C : w.end; p = !u(p) || C < p ? C : p; a = !u(a) || w > a ? w : a; t = t +
                    1 + e.descendants; H = Math.max(e.height + 1, H); return e
            }); b && (b.start = z(b.start, p), b.end = z(b.end, a)); l(y, { children: E, descendants: t, height: H }); "function" === typeof I && I(y, m); return y
        }; return { getListOfParents: x, getNode: A, getTree: function (l, y) { var k = l.map(function (b) { return b.id }); l = x(l, k); return A("", null, 1, null, l, y) } }
    }); J(c, "parts-gantt/TreeGridTick.js", [c["parts/Utilities.js"]], function (c) {
        var l = c.addEvent, u = c.defined, z = c.isObject, x = c.isNumber, A = c.pick, D = c.wrap, y; (function (k) {
            function b() {
                this.treeGrid ||
                (this.treeGrid = new c(this))
            } function g(b, p) {
                b = b.treeGrid; var a = !b.labelIcon, w = p.renderer, e = p.xy, C = p.options, q = C.width, f = C.height, d = e.x - q / 2 - C.padding; e = e.y - f / 2; var v = p.collapsed ? 90 : 180, G = p.show && x(e), B = b.labelIcon; B || (b.labelIcon = B = w.path(w.symbols[C.type](C.x, C.y, q, f)).addClass("highcharts-label-icon").add(p.group)); G || B.attr({ y: -9999 }); w.styledMode || B.attr({ "stroke-width": 1, fill: A(p.color, "#666666") }).css({ cursor: "pointer", stroke: C.lineColor, strokeWidth: C.lineWidth }); B[a ? "attr" : "animate"]({
                    translateX: d,
                    translateY: e, rotation: v
                })
            } function m(b, p, a, w, e, C, q, f, d) { var v = A(this.options && this.options.labels, C); C = this.pos; var G = this.axis, B = "treegrid" === G.options.type; b = b.apply(this, [p, a, w, e, v, q, f, d]); B && (p = v && z(v.symbol, !0) ? v.symbol : {}, v = v && x(v.indentation) ? v.indentation : 0, C = (C = (G = G.treeGrid.mapOfPosToGridNode) && G[C]) && C.depth || 1, b.x += p.width + 2 * p.padding + (C - 1) * v); return b } function t(b) {
                var p = this, a = p.pos, w = p.axis, e = p.label, C = w.treeGrid.mapOfPosToGridNode, q = w.options, f = A(p.options && p.options.labels, q && q.labels),
                d = f && z(f.symbol, !0) ? f.symbol : {}, v = (C = C && C[a]) && C.depth; q = "treegrid" === q.type; var G = -1 < w.tickPositions.indexOf(a); a = w.chart.styledMode; q && C && e && e.element && e.addClass("highcharts-treegrid-node-level-" + v); b.apply(p, Array.prototype.slice.call(arguments, 1)); q && e && e.element && C && C.descendants && 0 < C.descendants && (w = w.treeGrid.isCollapsed(C), g(p, { color: !a && e.styles && e.styles.color || "", collapsed: w, group: e.parentGroup, options: d, renderer: e.renderer, show: G, xy: e.xy }), d = "highcharts-treegrid-node-" + (w ? "expanded" :
                    "collapsed"), e.addClass("highcharts-treegrid-node-" + (w ? "collapsed" : "expanded")).removeClass(d), a || e.css({ cursor: "pointer" }), [e, p.treeGrid.labelIcon].forEach(function (d) {
                        d && !d.attachedTreeGridEvents && (l(d.element, "mouseover", function () { e.addClass("highcharts-treegrid-node-active"); e.renderer.styledMode || e.css({ textDecoration: "underline" }) }), l(d.element, "mouseout", function () { var d = u(f.style) ? f.style : {}; e.removeClass("highcharts-treegrid-node-active"); e.renderer.styledMode || e.css({ textDecoration: d.textDecoration }) }),
                            l(d.element, "click", function () { p.treeGrid.toggleCollapse() }), d.attachedTreeGridEvents = !0)
                    }))
            } var H = !1; k.compose = function (g) { H || (l(g, "init", b), D(g.prototype, "getLabelPosition", m), D(g.prototype, "renderLabel", t), g.prototype.collapse = function (b) { this.treeGrid.collapse(b) }, g.prototype.expand = function (b) { this.treeGrid.expand(b) }, g.prototype.toggleCollapse = function (b) { this.treeGrid.toggleCollapse(b) }, H = !0) }; var c = function () {
                function b(b) { this.tick = b } b.prototype.collapse = function (b) {
                    var a = this.tick, w = a.axis,
                    e = w.brokenAxis; e && w.treeGrid.mapOfPosToGridNode && (a = w.treeGrid.collapse(w.treeGrid.mapOfPosToGridNode[a.pos]), e.setBreaks(a, A(b, !0)))
                }; b.prototype.expand = function (b) { var a = this.tick, w = a.axis, e = w.brokenAxis; e && w.treeGrid.mapOfPosToGridNode && (a = w.treeGrid.expand(w.treeGrid.mapOfPosToGridNode[a.pos]), e.setBreaks(a, A(b, !0))) }; b.prototype.toggleCollapse = function (b) {
                    var a = this.tick, w = a.axis, e = w.brokenAxis; e && w.treeGrid.mapOfPosToGridNode && (a = w.treeGrid.toggleCollapse(w.treeGrid.mapOfPosToGridNode[a.pos]),
                        e.setBreaks(a, A(b, !0)))
                }; return b
            }(); k.Additions = c
        })(y || (y = {})); return y
    }); J(c, "mixins/tree-series.js", [c["parts/Color.js"], c["parts/Utilities.js"]], function (c, l) {
        var u = l.extend, z = l.isArray, x = l.isNumber, A = l.isObject, D = l.merge, y = l.pick; return {
            getColor: function (k, b) {
                var g = b.index, m = b.mapOptionsToLevel, t = b.parentColor, H = b.parentColorIndex, l = b.series, E = b.colors, p = b.siblings, a = l.points, w = l.chart.options.chart, e; if (k) {
                    a = a[k.i]; k = m[k.level] || {}; if (m = a && k.colorByPoint) {
                        var C = a.index % (E ? E.length : w.colorCount);
                        var q = E && E[C]
                    } if (!l.chart.styledMode) { E = a && a.options.color; w = k && k.color; if (e = t) e = (e = k && k.colorVariation) && "brightness" === e.key ? c.parse(t).brighten(g / p * e.to).get() : t; e = y(E, w, q, e, l.color) } var f = y(a && a.options.colorIndex, k && k.colorIndex, C, H, b.colorIndex)
                } return { color: e, colorIndex: f }
            }, getLevelOptions: function (k) {
                var b = null; if (A(k)) {
                    b = {}; var g = x(k.from) ? k.from : 1; var m = k.levels; var t = {}; var H = A(k.defaults) ? k.defaults : {}; z(m) && (t = m.reduce(function (b, m) {
                        if (A(m) && x(m.level)) {
                            var p = D({}, m); var a = "boolean" ===
                                typeof p.levelIsConstant ? p.levelIsConstant : H.levelIsConstant; delete p.levelIsConstant; delete p.level; m = m.level + (a ? 0 : g - 1); A(b[m]) ? u(b[m], p) : b[m] = p
                        } return b
                    }, {})); m = x(k.to) ? k.to : 1; for (k = 0; k <= m; k++)b[k] = D({}, H, A(t[k]) ? t[k] : {})
                } return b
            }, setTreeValues: function m(b, g) {
                var t = g.before, H = g.idRoot, c = g.mapIdToNode[H], E = g.points[b.i], p = E && E.options || {}, a = 0, w = []; u(b, {
                    levelDynamic: b.level - (("boolean" === typeof g.levelIsConstant ? g.levelIsConstant : 1) ? 0 : c.level), name: y(E && E.name, ""), visible: H === b.id || ("boolean" ===
                        typeof g.visible ? g.visible : !1)
                }); "function" === typeof t && (b = t(b, g)); b.children.forEach(function (e, C) { var q = u({}, g); u(q, { index: C, siblings: b.children.length, visible: b.visible }); e = m(e, q); w.push(e); e.visible && (a += e.val) }); b.visible = 0 < a || b.visible; t = y(p.value, a); u(b, { children: w, childrenTotal: a, isLeaf: b.visible && !a, val: t }); return b
            }, updateRootId: function (b) { if (A(b)) { var g = A(b.options) ? b.options : {}; g = y(b.rootNode, g.rootId, ""); A(b.userOptions) && (b.userOptions.rootId = g); b.rootNode = g } return g }
        }
    }); J(c, "parts-gantt/GridAxis.js",
        [c["parts/Axis.js"], c["parts/Globals.js"], c["parts/Options.js"], c["parts/Tick.js"], c["parts/Utilities.js"]], function (c, l, u, z, x) {
            var A = u.dateFormat, D = x.addEvent, y = x.defined, k = x.erase, b = x.find, g = x.isArray, m = x.isNumber, t = x.merge, H = x.pick, I = x.timeUnits, E = x.wrap; u = l.Chart; var p = function (e) { var a = e.options; a.labels || (a.labels = {}); a.labels.align = H(a.labels.align, "center"); e.categories || (a.showLastLabel = !1); e.labelRotation = 0; a.labels.rotation = 0 }; ""; c.prototype.getMaxLabelDimensions = function (e, a) {
                var q = {
                    width: 0,
                    height: 0
                }; a.forEach(function (f) { f = e[f]; if (x.isObject(f, !0)) { var d = x.isObject(f.label, !0) ? f.label : {}; f = d.getBBox ? d.getBBox().height : 0; d.textStr && !m(d.textPxLength) && (d.textPxLength = d.getBBox().width); d = m(d.textPxLength) ? Math.round(d.textPxLength) : 0; q.height = Math.max(f, q.height); q.width = Math.max(d, q.width) } }); return q
            }; l.dateFormats.W = function (e) {
                e = new this.Date(e); var a = (this.get("Day", e) + 6) % 7, q = new this.Date(e.valueOf()); this.set("Date", q, this.get("Date", e) - a + 3); a = new this.Date(this.get("FullYear",
                    q), 0, 1); 4 !== this.get("Day", a) && (this.set("Month", e, 0), this.set("Date", e, 1 + (11 - this.get("Day", a)) % 7)); return (1 + Math.floor((q.valueOf() - a.valueOf()) / 6048E5)).toString()
            }; l.dateFormats.E = function (a) { return A("%a", a, !0).charAt(0) }; D(u, "afterSetChartSize", function () { this.axes.forEach(function (a) { (a.grid && a.grid.columns || []).forEach(function (a) { a.setAxisSize(); a.setAxisTranslation() }) }) }); D(z, "afterGetLabelPosition", function (a) {
                var e = this.label, q = this.axis, f = q.reversed, d = q.chart, v = q.options.grid || {}, G =
                    q.options.labels, B = G.align, r = w.Side[q.side], h = a.tickmarkOffset, n = q.tickPositions, F = this.pos - h; n = m(n[a.index + 1]) ? n[a.index + 1] - h : q.max + h; var b = q.tickSize("tick"); h = b ? b[0] : 0; b = b ? b[1] / 2 : 0; if (!0 === v.enabled) {
                        if ("top" === r) { v = q.top + q.offset; var K = v - h } else "bottom" === r ? (K = d.chartHeight - q.bottom + q.offset, v = K + h) : (v = q.top + q.len - q.translate(f ? n : F), K = q.top + q.len - q.translate(f ? F : n)); "right" === r ? (r = d.chartWidth - q.right + q.offset, f = r + h) : "left" === r ? (f = q.left + q.offset, r = f - h) : (r = Math.round(q.left + q.translate(f ? n : F)) -
                            b, f = Math.round(q.left + q.translate(f ? F : n)) - b); this.slotWidth = f - r; a.pos.x = "left" === B ? r : "right" === B ? f : r + (f - r) / 2; a.pos.y = K + (v - K) / 2; d = d.renderer.fontMetrics(G.style.fontSize, e.element); e = e.getBBox().height; G.useHTML ? a.pos.y += d.b + -(e / 2) : (e = Math.round(e / d.h), a.pos.y += (d.b - (d.h - d.f)) / 2 + -((e - 1) * d.h / 2)); a.pos.x += q.horiz && G.x || 0
                    }
            }); var a = function () {
                function a(a) { this.axis = a } a.prototype.isOuterAxis = function () {
                    var a = this.axis, e = a.grid.columnIndex, f = a.linkedParent && a.linkedParent.grid.columns || a.grid.columns, d =
                        e ? a.linkedParent : a, v = -1, G = 0; a.chart[a.coll].forEach(function (f, r) { f.side !== a.side || f.options.isInternal || (G = r, f === d && (v = r)) }); return G === v && (m(e) ? f.length === e : !0)
                }; return a
            }(), w = function () {
                function e() { } e.compose = function (a) {
                    c.keepProps.push("grid"); E(a.prototype, "unsquish", e.wrapUnsquish); D(a, "init", e.onInit); D(a, "afterGetOffset", e.onAfterGetOffset); D(a, "afterGetTitlePosition", e.onAfterGetTitlePosition); D(a, "afterInit", e.onAfterInit); D(a, "afterRender", e.onAfterRender); D(a, "afterSetAxisTranslation",
                        e.onAfterSetAxisTranslation); D(a, "afterSetOptions", e.onAfterSetOptions); D(a, "afterSetOptions", e.onAfterSetOptions2); D(a, "afterSetScale", e.onAfterSetScale); D(a, "afterTickSize", e.onAfterTickSize); D(a, "trimTicks", e.onTrimTicks); D(a, "destroy", e.onDestroy)
                }; e.onAfterGetOffset = function () { var a = this.grid; (a && a.columns || []).forEach(function (a) { a.getOffset() }) }; e.onAfterGetTitlePosition = function (a) {
                    if (!0 === (this.options.grid || {}).enabled) {
                        var q = this.axisTitle, f = this.height, d = this.horiz, v = this.left, G = this.offset,
                        B = this.opposite, r = this.options.title, h = void 0 === r ? {} : r; r = this.top; var n = this.width, F = this.tickSize(), b = q && q.getBBox().width, K = h.x || 0, w = h.y || 0, C = H(h.margin, d ? 5 : 10); q = this.chart.renderer.fontMetrics(h.style && h.style.fontSize, q).f; F = (d ? r + f : v) + (d ? 1 : -1) * (B ? -1 : 1) * (F ? F[0] / 2 : 0) + (this.side === e.Side.bottom ? q : 0); a.titlePosition.x = d ? v - b / 2 - C + K : F + (B ? n : 0) + G + K; a.titlePosition.y = d ? F - (B ? f : 0) + (B ? q : -q) / 2 + G + w : r - C + w
                    }
                }; e.onAfterInit = function () {
                    var a = this.chart, e = this.options.grid; e = void 0 === e ? {} : e; var f = this.userOptions;
                    e.enabled && (p(this), E(this, "labelFormatter", function (d) { var a = this.axis, h = this.value, n = a.tickPositions, f = (a.isLinked ? a.linkedParent : a).series[0], e = h === n[0]; n = h === n[n.length - 1]; f = f && b(f.options.data, function (n) { return n[a.isXAxis ? "x" : "y"] === h }); this.isFirst = e; this.isLast = n; this.point = f; return d.call(this) })); if (e.columns) for (var d = this.grid.columns = [], v = this.grid.columnIndex = 0; ++v < e.columns.length;) {
                        var G = t(f, e.columns[e.columns.length - v - 1], { linkedTo: 0, type: "category" }); delete G.grid.columns; G = new c(this.chart,
                            G); G.grid.isColumn = !0; G.grid.columnIndex = v; k(a.axes, G); k(a[this.coll], G); d.push(G)
                    }
                }; e.onAfterRender = function () {
                    var a = this.grid, q = this.options, f = this.chart.renderer; if (!0 === (q.grid || {}).enabled) {
                        this.maxLabelDimensions = this.getMaxLabelDimensions(this.ticks, this.tickPositions); this.rightWall && this.rightWall.destroy(); if (this.grid && this.grid.isOuterAxis() && this.axisLine) {
                            var d = q.lineWidth; if (d) {
                                var v = this.getLinePath(d), b = v[0], B = v[1], r = ((this.tickSize("tick") || [1])[0] - 1) * (this.side === e.Side.top || this.side ===
                                    e.Side.left ? -1 : 1); "M" === b[0] && "L" === B[0] && (this.horiz ? (b[2] += r, B[2] += r) : (b[1] += r, B[1] += r)); this.grid.axisLineExtra ? this.grid.axisLineExtra.animate({ d: v }) : (this.grid.axisLineExtra = f.path(v).attr({ zIndex: 7 }).addClass("highcharts-axis-line").add(this.axisGroup), f.styledMode || this.grid.axisLineExtra.attr({ stroke: q.lineColor, "stroke-width": d })); this.axisLine[this.showAxis ? "show" : "hide"](!0)
                            }
                        } (a && a.columns || []).forEach(function (h) { h.render() })
                    }
                }; e.onAfterSetAxisTranslation = function () {
                    var a = this.tickPositions &&
                        this.tickPositions.info, e = this.options, f = e.grid || {}, d = this.userOptions.labels || {}; this.horiz && (!0 === f.enabled && this.series.forEach(function (d) { d.options.pointRange = 0 }), a && e.dateTimeLabelFormats && e.labels && !y(d.align) && (!1 === e.dateTimeLabelFormats[a.unitName].range || 1 < a.count) && (e.labels.align = "left", y(d.x) || (e.labels.x = 3)))
                }; e.onAfterSetOptions = function (a) {
                    var e = this.options; a = a.userOptions; var f = e && x.isObject(e.grid, !0) ? e.grid : {}; if (!0 === f.enabled) {
                        var d = t(!0, {
                            className: "highcharts-grid-axis " + (a.className ||
                                ""), dateTimeLabelFormats: { hour: { list: ["%H:%M", "%H"] }, day: { list: ["%A, %e. %B", "%a, %e. %b", "%E"] }, week: { list: ["Week %W", "W%W"] }, month: { list: ["%B", "%b", "%o"] } }, grid: { borderWidth: 1 }, labels: { padding: 2, style: { fontSize: "13px" } }, margin: 0, title: { text: null, reserveSpace: !1, rotation: 0 }, units: [["millisecond", [1, 10, 100]], ["second", [1, 10]], ["minute", [1, 5, 15]], ["hour", [1, 6]], ["day", [1]], ["week", [1]], ["month", [1]], ["year", null]]
                        }, a); "xAxis" === this.coll && (y(a.linkedTo) && !y(a.tickPixelInterval) && (d.tickPixelInterval =
                            350), y(a.tickPixelInterval) || !y(a.linkedTo) || y(a.tickPositioner) || y(a.tickInterval) || (d.tickPositioner = function (a, f) { var e = this.linkedParent && this.linkedParent.tickPositions && this.linkedParent.tickPositions.info; if (e) { var r, h = d.units; for (r = 0; r < h.length; r++)if (h[r][0] === e.unitName) { var n = r; break } if (h[n + 1]) { var F = h[n + 1][0]; var v = (h[n + 1][1] || [1])[0] } else "year" === e.unitName && (F = "year", v = 10 * e.count); e = I[F]; this.tickInterval = e * v; return this.getTimeTicks({ unitRange: e, count: v, unitName: F }, a, f, this.options.startOfWeek) } }));
                        t(!0, this.options, d); this.horiz && (e.minPadding = H(a.minPadding, 0), e.maxPadding = H(a.maxPadding, 0)); m(e.grid.borderWidth) && (e.tickWidth = e.lineWidth = f.borderWidth)
                    }
                }; e.onAfterSetOptions2 = function (a) { a = (a = a.userOptions) && a.grid || {}; var e = a.columns; a.enabled && e && t(!0, this.options, e[e.length - 1]) }; e.onAfterSetScale = function () { (this.grid.columns || []).forEach(function (a) { a.setScale() }) }; e.onAfterTickSize = function (a) {
                    var e = c.defaultLeftAxisOptions, f = this.horiz, d = this.maxLabelDimensions, v = this.options.grid; v =
                        void 0 === v ? {} : v; v.enabled && d && (e = 2 * Math.abs(e.labels.x), f = f ? v.cellHeight || e + d.height : e + d.width, g(a.tickSize) ? a.tickSize[0] = f : a.tickSize = [f, 0])
                }; e.onDestroy = function (a) { var e = this.grid; (e.columns || []).forEach(function (f) { f.destroy(a.keepEvents) }); e.columns = void 0 }; e.onInit = function (e) { e = e.userOptions || {}; var b = e.grid || {}; b.enabled && y(b.borderColor) && (e.tickColor = e.lineColor = b.borderColor); this.grid || (this.grid = new a(this)) }; e.onTrimTicks = function () {
                    var a = this.options, e = this.categories, f = this.tickPositions,
                    d = f[0], v = f[f.length - 1], b = this.linkedParent && this.linkedParent.min || this.min, B = this.linkedParent && this.linkedParent.max || this.max, r = this.tickInterval; !0 !== (a.grid || {}).enabled || e || !this.horiz && !this.isLinked || (d < b && d + r > b && !a.startOnTick && (f[0] = b), v > B && v - r < B && !a.endOnTick && (f[f.length - 1] = B))
                }; e.wrapUnsquish = function (a) { var e = this.options.grid; return !0 === (void 0 === e ? {} : e).enabled && this.categories ? this.tickInterval : a.apply(this, Array.prototype.slice.call(arguments, 1)) }; return e
            }(); (function (a) {
                a = a.Side ||
                (a.Side = {}); a[a.top = 0] = "top"; a[a.right = 1] = "right"; a[a.bottom = 2] = "bottom"; a[a.left = 3] = "left"
            })(w || (w = {})); w.compose(c); return w
        }); J(c, "modules/broken-axis.src.js", [c["parts/Axis.js"], c["parts/Globals.js"], c["parts/Utilities.js"], c["parts/Stacking.js"]], function (c, l, u, z) {
            var x = u.addEvent, A = u.find, D = u.fireEvent, y = u.isArray, k = u.isNumber, b = u.pick, g = l.Series, m = function () {
                function g(b) { this.hasBreaks = !1; this.axis = b } g.isInBreak = function (b, g) {
                    var m = b.repeat || Infinity, p = b.from, a = b.to - b.from; g = g >= p ? (g - p) % m :
                        m - (p - g) % m; return b.inclusive ? g <= a : g < a && 0 !== g
                }; g.lin2Val = function (b) { var m = this.brokenAxis; m = m && m.breakArray; if (!m) return b; var k; for (k = 0; k < m.length; k++) { var p = m[k]; if (p.from >= b) break; else p.to < b ? b += p.len : g.isInBreak(p, b) && (b += p.len) } return b }; g.val2Lin = function (b) { var m = this.brokenAxis; m = m && m.breakArray; if (!m) return b; var k = b, p; for (p = 0; p < m.length; p++) { var a = m[p]; if (a.to <= b) k -= a.len; else if (a.from >= b) break; else if (g.isInBreak(a, b)) { k -= b - a.from; break } } return k }; g.prototype.findBreakAt = function (b, g) {
                    return A(g,
                        function (g) { return g.from < b && b < g.to })
                }; g.prototype.isInAnyBreak = function (m, k) { var t = this.axis, p = t.options.breaks, a = p && p.length, w; if (a) { for (; a--;)if (g.isInBreak(p[a], m)) { var e = !0; w || (w = b(p[a].showPoints, !t.isXAxis)) } var C = e && k ? e && !w : e } return C }; g.prototype.setBreaks = function (m, k) {
                    var t = this, p = t.axis, a = y(m) && !!m.length; p.isDirty = t.hasBreaks !== a; t.hasBreaks = a; p.options.breaks = p.userOptions.breaks = m; p.forceRedraw = !0; p.series.forEach(function (a) { a.isDirty = !0 }); a || p.val2lin !== g.val2Lin || (delete p.val2lin,
                        delete p.lin2val); a && (p.userOptions.ordinal = !1, p.lin2val = g.lin2Val, p.val2lin = g.val2Lin, p.setExtremes = function (a, e, b, q, f) { if (t.hasBreaks) { for (var d, v = this.options.breaks; d = t.findBreakAt(a, v);)a = d.to; for (; d = t.findBreakAt(e, v);)e = d.from; e < a && (e = a) } c.prototype.setExtremes.call(this, a, e, b, q, f) }, p.setAxisTranslation = function (a) {
                            c.prototype.setAxisTranslation.call(this, a); t.unitLength = null; if (t.hasBreaks) {
                                a = p.options.breaks || []; var e = [], w = [], q = 0, f, d = p.userMin || p.min, v = p.userMax || p.max, G = b(p.pointRangePadding,
                                    0), B; a.forEach(function (a) { f = a.repeat || Infinity; g.isInBreak(a, d) && (d += a.to % f - d % f); g.isInBreak(a, v) && (v -= v % f - a.from % f) }); a.forEach(function (a) { h = a.from; for (f = a.repeat || Infinity; h - f > d;)h -= f; for (; h < d;)h += f; for (B = h; B < v; B += f)e.push({ value: B, move: "in" }), e.push({ value: B + (a.to - a.from), move: "out", size: a.breakSize }) }); e.sort(function (a, h) { return a.value === h.value ? ("in" === a.move ? 0 : 1) - ("in" === h.move ? 0 : 1) : a.value - h.value }); var r = 0; var h = d; e.forEach(function (a) {
                                        r += "in" === a.move ? 1 : -1; 1 === r && "in" === a.move && (h = a.value);
                                        0 === r && (w.push({ from: h, to: a.value, len: a.value - h - (a.size || 0) }), q += a.value - h - (a.size || 0))
                                    }); p.breakArray = t.breakArray = w; t.unitLength = v - d - q + G; D(p, "afterBreaks"); p.staticScale ? p.transA = p.staticScale : t.unitLength && (p.transA *= (v - p.min + G) / t.unitLength); G && (p.minPixelPadding = p.transA * p.minPointOffset); p.min = d; p.max = v
                            }
                        }); b(k, !0) && p.chart.redraw()
                }; return g
            }(); l = function () {
                function t() { } t.compose = function (t, c) {
                    t.keepProps.push("brokenAxis"); var l = g.prototype; l.drawBreaks = function (g, a) {
                        var w = this, e = w.points,
                        m, q, f, d; if (g && g.brokenAxis && g.brokenAxis.hasBreaks) { var v = g.brokenAxis; a.forEach(function (a) { m = v && v.breakArray || []; q = g.isXAxis ? g.min : b(w.options.threshold, g.min); e.forEach(function (e) { d = b(e["stack" + a.toUpperCase()], e[a]); m.forEach(function (a) { if (k(q) && k(d)) { f = !1; if (q < a.from && d > a.to || q > a.from && d < a.from) f = "pointBreak"; else if (q < a.from && d > a.from && d < a.to || q > a.from && d > a.to && d < a.from) f = "pointInBreak"; f && D(g, f, { point: e, brk: a }) } }) }) }) }
                    }; l.gappedPath = function () {
                        var b = this.currentDataGrouping, a = b && b.gapSize;
                        b = this.options.gapSize; var w = this.points.slice(), e = w.length - 1, g = this.yAxis, q; if (b && 0 < e) for ("value" !== this.options.gapUnit && (b *= this.basePointRange), a && a > b && a >= this.basePointRange && (b = a), q = void 0; e--;)q && !1 !== q.visible || (q = w[e + 1]), a = w[e], !1 !== q.visible && !1 !== a.visible && (q.x - a.x > b && (q = (a.x + q.x) / 2, w.splice(e + 1, 0, { isNull: !0, x: q }), g.stacking && this.options.stacking && (q = g.stacking.stacks[this.stackKey][q] = new z(g, g.options.stackLabels, !1, q, this.stack), q.total = 0)), q = a); return this.getGraphPath(w)
                    }; x(t, "init",
                        function () { this.brokenAxis || (this.brokenAxis = new m(this)) }); x(t, "afterInit", function () { "undefined" !== typeof this.brokenAxis && this.brokenAxis.setBreaks(this.options.breaks, !1) }); x(t, "afterSetTickPositions", function () { var b = this.brokenAxis; if (b && b.hasBreaks) { var a = this.tickPositions, g = this.tickPositions.info, e = [], m; for (m = 0; m < a.length; m++)b.isInAnyBreak(a[m]) || e.push(a[m]); this.tickPositions = e; this.tickPositions.info = g } }); x(t, "afterSetOptions", function () {
                            this.brokenAxis && this.brokenAxis.hasBreaks && (this.options.ordinal =
                                !1)
                        }); x(c, "afterGeneratePoints", function () { var b = this.options.connectNulls, a = this.points, g = this.xAxis, e = this.yAxis; if (this.isDirty) for (var m = a.length; m--;) { var q = a[m], f = !(null === q.y && !1 === b) && (g && g.brokenAxis && g.brokenAxis.isInAnyBreak(q.x, !0) || e && e.brokenAxis && e.brokenAxis.isInAnyBreak(q.y, !0)); q.visible = f ? !1 : !1 !== q.options.visible } }); x(c, "afterRender", function () { this.drawBreaks(this.xAxis, ["x"]); this.drawBreaks(this.yAxis, b(this.pointArrayMap, ["y"])) })
                }; return t
            }(); l.compose(c, g); return l
        }); J(c,
            "parts-gantt/TreeGridAxis.js", [c["parts/Axis.js"], c["parts/Tick.js"], c["parts-gantt/Tree.js"], c["parts-gantt/TreeGridTick.js"], c["mixins/tree-series.js"], c["parts/Utilities.js"]], function (c, l, u, z, x, A) {
                var D = A.addEvent, y = A.find, k = A.fireEvent, b = A.isNumber, g = A.isObject, m = A.isString, t = A.merge, H = A.pick, I = A.wrap, E; (function (p) {
                    function a(a, d) { var h = a.collapseStart || 0; a = a.collapseEnd || 0; a >= d && (h -= .5); return { from: h, to: a, showPoints: !1 } } function w(a, d, h) {
                        var n = [], f = [], e = {}, b = {}, r = -1, v = "boolean" === typeof d ?
                            d : !1; a = u.getTree(a, {
                                after: function (a) { a = b[a.pos]; var h = 0, n = 0; a.children.forEach(function (a) { n += (a.descendants || 0) + 1; h = Math.max((a.height || 0) + 1, h) }); a.descendants = n; a.height = h; a.collapsed && f.push(a) }, before: function (a) {
                                    var h = g(a.data, !0) ? a.data : {}, d = m(h.name) ? h.name : "", f = e[a.parent]; f = g(f, !0) ? b[f.pos] : null; var F = function (a) { return a.name === d }, B; v && g(f, !0) && (B = y(f.children, F)) ? (F = B.pos, B.nodes.push(a)) : F = r++; b[F] || (b[F] = B = { depth: f ? f.depth + 1 : 0, name: d, nodes: [a], children: [], pos: F }, -1 !== F && n.push(d), g(f,
                                        !0) && f.children.push(B)); m(a.id) && (e[a.id] = a); B && !0 === h.collapsed && (B.collapsed = !0); a.pos = F
                                }
                            }); b = function (a, h) { var n = function (a, d, f) { var e = d + (-1 === d ? 0 : h - 1), b = (e - d) / 2, r = d + b; a.nodes.forEach(function (a) { var h = a.data; g(h, !0) && (h.y = d + (h.seriesIndex || 0), delete h.seriesIndex); a.pos = r }); f[r] = a; a.pos = r; a.tickmarkOffset = b + .5; a.collapseStart = e + .5; a.children.forEach(function (a) { n(a, e + 1, f); e = (a.collapseEnd || 0) - .5 }); a.collapseEnd = e + .5; return f }; return n(a["-1"], -1, {}) }(b, h); return {
                                categories: n, mapOfIdToNode: e,
                                mapOfPosToGridNode: b, collapsedNodes: f, tree: a
                            }
                    } function e(a) {
                        a.target.axes.filter(function (a) { return "treegrid" === a.options.type }).forEach(function (d) {
                            var h = d.options || {}, n = h.labels, f = h.uniqueNames, e = 0; if (!d.treeGrid.mapOfPosToGridNode || d.series.some(function (a) { return !a.hasRendered || a.isDirtyData || a.isDirty })) h = d.series.reduce(function (a, h) { h.visible && ((h.options.data || []).forEach(function (h) { g(h, !0) && (h.seriesIndex = e, a.push(h)) }), !0 === f && e++); return a }, []), h = w(h, f || !1, !0 === f ? e : 1), d.categories = h.categories,
                                d.treeGrid.mapOfPosToGridNode = h.mapOfPosToGridNode, d.hasNames = !0, d.treeGrid.tree = h.tree, d.series.forEach(function (a) { var h = (a.options.data || []).map(function (a) { return g(a, !0) ? t(a) : a }); a.visible && a.setData(h, !1) }), d.treeGrid.mapOptionsToLevel = x.getLevelOptions({ defaults: n, from: 1, levels: n && n.levels, to: d.treeGrid.tree && d.treeGrid.tree.height }), "beforeRender" === a.type && (d.treeGrid.collapsedNodes = h.collapsedNodes)
                        })
                    } function c(a, d) {
                        var h = this.treeGrid.mapOptionsToLevel || {}, n = this.ticks, f = n[d], e; if ("treegrid" ===
                            this.options.type && this.treeGrid.mapOfPosToGridNode) { var b = this.treeGrid.mapOfPosToGridNode[d]; (h = h[b.depth]) && (e = { labels: h }); f ? (f.parameters.category = b.name, f.options = e, f.addLabel()) : n[d] = new l(this, d, void 0, void 0, { category: b.name, tickmarkOffset: b.tickmarkOffset, options: e }) } else a.apply(this, Array.prototype.slice.call(arguments, 1))
                    } function q(a) {
                        var d = this.options; d = (d = d && d.labels) && b(d.indentation) ? d.indentation : 0; var h = a.apply(this, Array.prototype.slice.call(arguments, 1)); if ("treegrid" === this.options.type &&
                            this.treeGrid.mapOfPosToGridNode) { var n = this.treeGrid.mapOfPosToGridNode[-1].height || 0; h.width += d * (n - 1) } return h
                    } function f(a, d, h) {
                        var n = this, f = "treegrid" === h.type; n.treeGrid || (n.treeGrid = new G(n)); f && (D(d, "beforeRender", e), D(d, "beforeRedraw", e), D(d, "addSeries", function (a) { a.options.data && (a = w(a.options.data, h.uniqueNames || !1, 1), n.treeGrid.collapsedNodes = (n.treeGrid.collapsedNodes || []).concat(a.collapsedNodes)) }), D(n, "foundExtremes", function () {
                            n.treeGrid.collapsedNodes && n.treeGrid.collapsedNodes.forEach(function (a) {
                                var h =
                                    n.treeGrid.collapse(a); n.brokenAxis && (n.brokenAxis.setBreaks(h, !1), n.treeGrid.collapsedNodes && (n.treeGrid.collapsedNodes = n.treeGrid.collapsedNodes.filter(function (h) { return a.collapseStart !== h.collapseStart || a.collapseEnd !== h.collapseEnd })))
                            })
                        }), D(n, "afterBreaks", function () { var a; "yAxis" === n.coll && !n.staticScale && (null === (a = n.chart.options.chart) || void 0 === a ? 0 : a.height) && (n.isDirty = !0) }), h = t({
                            grid: { enabled: !0 }, labels: {
                                align: "left", levels: [{ level: void 0 }, { level: 1, style: { fontWeight: "bold" } }], symbol: {
                                    type: "triangle",
                                    x: -5, y: -5, height: 10, width: 10, padding: 5
                                }
                            }, uniqueNames: !1
                        }, h, { reversed: !0, grid: { columns: void 0 } })); a.apply(n, [d, h]); f && (n.hasNames = !0, n.options.showLastLabel = !0)
                    } function d(a) {
                        var d = this.options; "treegrid" === d.type ? (this.min = H(this.userMin, d.min, this.dataMin), this.max = H(this.userMax, d.max, this.dataMax), k(this, "foundExtremes"), this.setAxisTranslation(!0), this.tickmarkOffset = .5, this.tickInterval = 1, this.tickPositions = this.treeGrid.mapOfPosToGridNode ? this.treeGrid.getTickPositions() : []) : a.apply(this, Array.prototype.slice.call(arguments,
                            1))
                    } var v = !1; p.compose = function (a) { v || (I(a.prototype, "generateTick", c), I(a.prototype, "getMaxLabelDimensions", q), I(a.prototype, "init", f), I(a.prototype, "setTickInterval", d), z.compose(l), v = !0) }; var G = function () {
                        function d(a) { this.axis = a } d.prototype.collapse = function (d) { var h = this.axis, n = h.options.breaks || []; d = a(d, h.max); n.push(d); return n }; d.prototype.expand = function (d) {
                            var h = this.axis, n = h.options.breaks || [], f = a(d, h.max); return n.reduce(function (a, d) { d.to === f.to && d.from === f.from || a.push(d); return a },
                                [])
                        }; d.prototype.getTickPositions = function () { var a = this.axis; return Object.keys(a.treeGrid.mapOfPosToGridNode || {}).reduce(function (d, n) { n = +n; !(a.min <= n && a.max >= n) || a.brokenAxis && a.brokenAxis.isInAnyBreak(n) || d.push(n); return d }, []) }; d.prototype.isCollapsed = function (d) { var h = this.axis, n = h.options.breaks || [], f = a(d, h.max); return n.some(function (a) { return a.from === f.from && a.to === f.to }) }; d.prototype.toggleCollapse = function (a) { return this.isCollapsed(a) ? this.expand(a) : this.collapse(a) }; return d
                    }(); p.Additions =
                        G
                })(E || (E = {})); c.prototype.utils = { getNode: u.getNode }; E.compose(c); return E
            }); J(c, "parts-gantt/CurrentDateIndicator.js", [c["parts/Globals.js"], c["parts/Options.js"], c["parts/Utilities.js"], c["parts/PlotLineOrBand.js"]], function (c, l, u, z) {
                var x = l.dateFormat; l = u.addEvent; var A = u.merge; u = u.wrap; var D = { currentDateIndicator: !0, color: "#ccd6eb", width: 2, label: { format: "%a, %b %d %Y, %H:%M", formatter: function (c, k) { return x(k, c) }, rotation: 0, style: { fontSize: "10px" } } }; l(c.Axis, "afterSetOptions", function () {
                    var c =
                        this.options, k = c.currentDateIndicator; k && (k = "object" === typeof k ? A(D, k) : A(D), k.value = new Date, c.plotLines || (c.plotLines = []), c.plotLines.push(k))
                }); l(z, "render", function () { this.label && this.label.attr({ text: this.getLabelText(this.options.label) }) }); u(z.prototype, "getLabelText", function (c, k) { var b = this.options; return b.currentDateIndicator && b.label && "function" === typeof b.label.formatter ? (b.value = new Date, b.label.formatter.call(this, b.value, b.label.format)) : c.call(this, k) })
            }); J(c, "modules/static-scale.src.js",
                [c["parts/Globals.js"], c["parts/Utilities.js"]], function (c, l) {
                    var u = l.addEvent, z = l.defined, x = l.isNumber, A = l.pick; l = c.Chart; u(c.Axis, "afterSetOptions", function () { var c = this.chart.options && this.chart.options.chart; !this.horiz && x(this.options.staticScale) && (!c.height || c.scrollablePlotArea && c.scrollablePlotArea.minHeight) && (this.staticScale = this.options.staticScale) }); l.prototype.adjustHeight = function () {
                        "adjustHeight" !== this.redrawTrigger && ((this.axes || []).forEach(function (c) {
                            var l = c.chart, k = !!l.initiatedScale &&
                                l.options.animation, b = c.options.staticScale; if (c.staticScale && z(c.min)) { var g = A(c.brokenAxis && c.brokenAxis.unitLength, c.max + c.tickInterval - c.min) * b; g = Math.max(g, b); b = g - l.plotHeight; 1 <= Math.abs(b) && (l.plotHeight = g, l.redrawTrigger = "adjustHeight", l.setSize(void 0, l.chartHeight + b, k)); c.series.forEach(function (b) { (b = b.sharedClipKey && l[b.sharedClipKey]) && b.attr({ height: l.plotHeight }) }) }
                        }), this.initiatedScale = !0); this.redrawTrigger = null
                    }; u(l, "render", l.prototype.adjustHeight)
                }); J(c, "parts-gantt/PathfinderAlgorithms.js",
                    [c["parts/Utilities.js"]], function (c) {
                        function l(b, m, c) { c = c || 0; var g = b.length - 1; m -= 1e-7; for (var k, t; c <= g;)if (k = g + c >> 1, t = m - b[k].xMin, 0 < t) c = k + 1; else if (0 > t) g = k - 1; else return k; return 0 < c ? c - 1 : 0 } function u(b, m) { for (var g = l(b, m.x + 1) + 1; g--;) { var c; if (c = b[g].xMax >= m.x) c = b[g], c = m.x <= c.xMax && m.x >= c.xMin && m.y <= c.yMax && m.y >= c.yMin; if (c) return g } return -1 } function z(b) { var g = []; if (b.length) { g.push(["M", b[0].start.x, b[0].start.y]); for (var c = 0; c < b.length; ++c)g.push(["L", b[c].end.x, b[c].end.y]) } return g } function x(b,
                            c) { b.yMin = k(b.yMin, c.yMin); b.yMax = y(b.yMax, c.yMax); b.xMin = k(b.xMin, c.xMin); b.xMax = y(b.xMax, c.xMax) } var A = c.extend, D = c.pick, y = Math.min, k = Math.max, b = Math.abs; return {
                                straight: function (b, c) { return { path: [["M", b.x, b.y], ["L", c.x, c.y]], obstacles: [{ start: b, end: c }] } }, simpleConnect: A(function (g, c, k) {
                                    function m(a, f, d, b, e) { a = { x: a.x, y: a.y }; a[f] = d[b || f] + (e || 0); return a } function l(a, f, d) { var e = b(f[d] - a[d + "Min"]) > b(f[d] - a[d + "Max"]); return m(f, d, a, d + (e ? "Max" : "Min"), e ? 1 : -1) } var t = [], p = D(k.startDirectionX, b(c.x - g.x) >
                                        b(c.y - g.y)) ? "x" : "y", a = k.chartObstacles, w = u(a, g); k = u(a, c); if (-1 < k) { var e = a[k]; k = l(e, c, p); e = { start: k, end: c }; var C = k } else C = c; -1 < w && (a = a[w], k = l(a, g, p), t.push({ start: g, end: k }), k[p] >= g[p] === k[p] >= C[p] && (p = "y" === p ? "x" : "y", c = g[p] < c[p], t.push({ start: k, end: m(k, p, a, p + (c ? "Max" : "Min"), c ? 1 : -1) }), p = "y" === p ? "x" : "y")); g = t.length ? t[t.length - 1].end : g; k = m(g, p, C); t.push({ start: g, end: k }); p = m(k, "y" === p ? "x" : "y", C); t.push({ start: k, end: p }); t.push(e); return { path: z(t), obstacles: t }
                                }, { requiresObstacles: !0 }), fastAvoid: A(function (g,
                                    c, t) {
                                        function m(a, d, h) { var n, f = a.x < d.x ? 1 : -1; if (a.x < d.x) { var b = a; var e = d } else b = d, e = a; if (a.y < d.y) { var v = a; var F = d } else v = d, F = a; for (n = 0 > f ? y(l(B, e.x), B.length - 1) : 0; B[n] && (0 < f && B[n].xMin <= e.x || 0 > f && B[n].xMax >= b.x);) { if (B[n].xMin <= e.x && B[n].xMax >= b.x && B[n].yMin <= F.y && B[n].yMax >= v.y) return h ? { y: a.y, x: a.x < d.x ? B[n].xMin - 1 : B[n].xMax + 1, obstacle: B[n] } : { x: a.x, y: a.y < d.y ? B[n].yMin - 1 : B[n].yMax + 1, obstacle: B[n] }; n += f } return d } function A(a, d, h, f, e) {
                                            var n = e.soft, v = e.hard, r = f ? "x" : "y", c = { x: d.x, y: d.y }, B = { x: d.x, y: d.y };
                                            e = a[r + "Max"] >= n[r + "Max"]; n = a[r + "Min"] <= n[r + "Min"]; var G = a[r + "Max"] >= v[r + "Max"]; v = a[r + "Min"] <= v[r + "Min"]; var F = b(a[r + "Min"] - d[r]), g = b(a[r + "Max"] - d[r]); h = 10 > b(F - g) ? d[r] < h[r] : g < F; B[r] = a[r + "Min"]; c[r] = a[r + "Max"]; a = m(d, B, f)[r] !== B[r]; d = m(d, c, f)[r] !== c[r]; h = a ? d ? h : !0 : d ? !1 : h; h = n ? e ? h : !0 : e ? !1 : h; return v ? G ? h : !0 : G ? !1 : h
                                        } function E(a, h, b) {
                                            if (a.x === h.x && a.y === h.y) return []; var n = b ? "x" : "y", e = t.obstacleOptions.margin; var r = { soft: { xMin: f, xMax: d, yMin: v, yMax: G }, hard: t.hardBounds }; var c = u(B, a); if (-1 < c) {
                                                c = B[c]; r = A(c, a,
                                                    h, b, r); x(c, t.hardBounds); var g = b ? { y: a.y, x: c[r ? "xMax" : "xMin"] + (r ? 1 : -1) } : { x: a.x, y: c[r ? "yMax" : "yMin"] + (r ? 1 : -1) }; var F = u(B, g); -1 < F && (F = B[F], x(F, t.hardBounds), g[n] = r ? k(c[n + "Max"] - e + 1, (F[n + "Min"] + c[n + "Max"]) / 2) : y(c[n + "Min"] + e - 1, (F[n + "Max"] + c[n + "Min"]) / 2), a.x === g.x && a.y === g.y ? (C && (g[n] = r ? k(c[n + "Max"], F[n + "Max"]) + 1 : y(c[n + "Min"], F[n + "Min"]) - 1), C = !C) : C = !1); a = [{ start: a, end: g }]
                                            } else n = m(a, { x: b ? h.x : a.x, y: b ? a.y : h.y }, b), a = [{ start: a, end: { x: n.x, y: n.y } }], n[b ? "x" : "y"] !== h[b ? "x" : "y"] && (r = A(n.obstacle, n, h, !b, r), x(n.obstacle,
                                                t.hardBounds), r = { x: b ? n.x : n.obstacle[r ? "xMax" : "xMin"] + (r ? 1 : -1), y: b ? n.obstacle[r ? "yMax" : "yMin"] + (r ? 1 : -1) : n.y }, b = !b, a = a.concat(E({ x: n.x, y: n.y }, r, b))); return a = a.concat(E(a[a.length - 1].end, h, !b))
                                        } function p(a, d, h) { var n = y(a.xMax - d.x, d.x - a.xMin) < y(a.yMax - d.y, d.y - a.yMin); h = A(a, d, h, n, { soft: t.hardBounds, hard: t.hardBounds }); return n ? { y: d.y, x: a[h ? "xMax" : "xMin"] + (h ? 1 : -1) } : { x: d.x, y: a[h ? "yMax" : "yMin"] + (h ? 1 : -1) } } var a = D(t.startDirectionX, b(c.x - g.x) > b(c.y - g.y)), w = a ? "x" : "y", e = [], C = !1, q = t.obstacleMetrics, f = y(g.x,
                                            c.x) - q.maxWidth - 10, d = k(g.x, c.x) + q.maxWidth + 10, v = y(g.y, c.y) - q.maxHeight - 10, G = k(g.y, c.y) + q.maxHeight + 10, B = t.chartObstacles; var r = l(B, f); q = l(B, d); B = B.slice(r, q + 1); if (-1 < (q = u(B, c))) { var h = p(B[q], c, g); e.push({ end: c, start: h }); c = h } for (; -1 < (q = u(B, c));)r = 0 > c[w] - g[w], h = { x: c.x, y: c.y }, h[w] = B[q][r ? w + "Max" : w + "Min"] + (r ? 1 : -1), e.push({ end: c, start: h }), c = h; g = E(g, c, a); g = g.concat(e.reverse()); return { path: z(g), obstacles: g }
                                }, { requiresObstacles: !0 })
                            }
                    }); J(c, "parts-gantt/ArrowSymbols.js", [c["parts/SVGRenderer.js"]], function (c) {
                        c.prototype.symbols.arrow =
                        function (c, u, z, x) { return [["M", c, u + x / 2], ["L", c + z, u], ["L", c, u + x / 2], ["L", c + z, u + x]] }; c.prototype.symbols["arrow-half"] = function (l, u, z, x) { return c.prototype.symbols.arrow(l, u, z / 2, x) }; c.prototype.symbols["triangle-left"] = function (c, u, z, x) { return [["M", c + z, u], ["L", c, u + x / 2], ["L", c + z, u + x], ["Z"]] }; c.prototype.symbols["arrow-filled"] = c.prototype.symbols["triangle-left"]; c.prototype.symbols["triangle-left-half"] = function (l, u, z, x) { return c.prototype.symbols["triangle-left"](l, u, z / 2, x) }; c.prototype.symbols["arrow-filled-half"] =
                            c.prototype.symbols["triangle-left-half"]
                    }); J(c, "parts-gantt/Pathfinder.js", [c["parts/Chart.js"], c["parts/Globals.js"], c["parts/Options.js"], c["parts/Point.js"], c["parts/Utilities.js"], c["parts-gantt/PathfinderAlgorithms.js"]], function (c, l, u, z, x, A) {
                        function D(a) { var d = a.shapeArgs; return d ? { xMin: d.x, xMax: d.x + d.width, yMin: d.y, yMax: d.y + d.height } : (d = a.graphic && a.graphic.getBBox()) ? { xMin: a.plotX - d.width / 2, xMax: a.plotX + d.width / 2, yMin: a.plotY - d.height / 2, yMax: a.plotY + d.height / 2 } : null } function y(b) {
                            for (var d =
                                b.length, f = 0, e, c, r = [], h = function (d, b, f) { f = a(f, 10); var n = d.yMax + f > b.yMin - f && d.yMin - f < b.yMax + f, e = d.xMax + f > b.xMin - f && d.xMin - f < b.xMax + f, c = n ? d.xMin > b.xMax ? d.xMin - b.xMax : b.xMin - d.xMax : Infinity, r = e ? d.yMin > b.yMax ? d.yMin - b.yMax : b.yMin - d.yMax : Infinity; return e && n ? f ? h(d, b, Math.floor(f / 2)) : Infinity : q(c, r) }; f < d; ++f)for (e = f + 1; e < d; ++e)c = h(b[f], b[e]), 80 > c && r.push(c); r.push(80); return C(Math.floor(r.sort(function (a, d) { return a - d })[Math.floor(r.length / 10)] / 2 - 1), 1)
                        } function k(a, d, b) { this.init(a, d, b) } function b(a) { this.init(a) }
                        function g(a) { if (a.options.pathfinder || a.series.reduce(function (a, b) { b.options && E(!0, b.options.connectors = b.options.connectors || {}, b.options.pathfinder); return a || b.options && b.options.pathfinder }, !1)) E(!0, a.options.connectors = a.options.connectors || {}, a.options.pathfinder), H('WARNING: Pathfinder options have been renamed. Use "chart.connectors" or "series.connectors" instead.') } ""; var m = x.addEvent, t = x.defined, H = x.error, I = x.extend, E = x.merge, p = x.objectEach, a = x.pick, w = x.splat, e = l.deg2rad, C = Math.max, q =
                            Math.min; I(u.defaultOptions, { connectors: { type: "straight", lineWidth: 1, marker: { enabled: !1, align: "center", verticalAlign: "middle", inside: !1, lineWidth: 1 }, startMarker: { symbol: "diamond" }, endMarker: { symbol: "arrow-filled" } } }); k.prototype = {
                                init: function (a, d, b) { this.fromPoint = a; this.toPoint = d; this.options = b; this.chart = a.series.chart; this.pathfinder = this.chart.pathfinder }, renderPath: function (a, d, b) {
                                    var e = this.chart, f = e.styledMode, c = e.pathfinder, h = !e.options.chart.forExport && !1 !== b, n = this.graphics && this.graphics.path;
                                    c.group || (c.group = e.renderer.g().addClass("highcharts-pathfinder-group").attr({ zIndex: -1 }).add(e.seriesGroup)); c.group.translate(e.plotLeft, e.plotTop); n && n.renderer || (n = e.renderer.path().add(c.group), f || n.attr({ opacity: 0 })); n.attr(d); a = { d: a }; f || (a.opacity = 1); n[h ? "animate" : "attr"](a, b); this.graphics = this.graphics || {}; this.graphics.path = n
                                }, addMarker: function (a, d, b) {
                                    var f = this.fromPoint.series.chart, c = f.pathfinder; f = f.renderer; var r = "start" === a ? this.fromPoint : this.toPoint, h = r.getPathfinderAnchorPoint(d);
                                    if (d.enabled && ((b = "start" === a ? b[1] : b[b.length - 2]) && "M" === b[0] || "L" === b[0])) {
                                        b = { x: b[1], y: b[2] }; b = r.getRadiansToVector(b, h); h = r.getMarkerVector(b, d.radius, h); b = -b / e; if (d.width && d.height) { var n = d.width; var v = d.height } else n = v = 2 * d.radius; this.graphics = this.graphics || {}; h = { x: h.x - n / 2, y: h.y - v / 2, width: n, height: v, rotation: b, rotationOriginX: h.x, rotationOriginY: h.y }; this.graphics[a] ? this.graphics[a].animate(h) : (this.graphics[a] = f.symbol(d.symbol).addClass("highcharts-point-connecting-path-" + a + "-marker").attr(h).add(c.group),
                                            f.styledMode || this.graphics[a].attr({ fill: d.color || this.fromPoint.color, stroke: d.lineColor, "stroke-width": d.lineWidth, opacity: 0 }).animate({ opacity: 1 }, r.series.options.animation))
                                    }
                                }, getPath: function (a) {
                                    var d = this.pathfinder, b = this.chart, e = d.algorithms[a.type], f = d.chartObstacles; if ("function" !== typeof e) H('"' + a.type + '" is not a Pathfinder algorithm.'); else return e.requiresObstacles && !f && (f = d.chartObstacles = d.getChartObstacles(a), b.options.connectors.algorithmMargin = a.algorithmMargin, d.chartObstacleMetrics =
                                        d.getObstacleMetrics(f)), e(this.fromPoint.getPathfinderAnchorPoint(a.startMarker), this.toPoint.getPathfinderAnchorPoint(a.endMarker), E({ chartObstacles: f, lineObstacles: d.lineObstacles || [], obstacleMetrics: d.chartObstacleMetrics, hardBounds: { xMin: 0, xMax: b.plotWidth, yMin: 0, yMax: b.plotHeight }, obstacleOptions: { margin: a.algorithmMargin }, startDirectionX: d.getAlgorithmStartDirection(a.startMarker) }, a))
                                }, render: function () {
                                    var a = this.fromPoint, d = a.series, b = d.chart, e = b.pathfinder, c = E(b.options.connectors, d.options.connectors,
                                        a.options.connectors, this.options), r = {}; b.styledMode || (r.stroke = c.lineColor || a.color, r["stroke-width"] = c.lineWidth, c.dashStyle && (r.dashstyle = c.dashStyle)); r["class"] = "highcharts-point-connecting-path highcharts-color-" + a.colorIndex; c = E(r, c); t(c.marker.radius) || (c.marker.radius = q(C(Math.ceil((c.algorithmMargin || 8) / 2) - 1, 1), 5)); a = this.getPath(c); b = a.path; a.obstacles && (e.lineObstacles = e.lineObstacles || [], e.lineObstacles = e.lineObstacles.concat(a.obstacles)); this.renderPath(b, r, d.options.animation); this.addMarker("start",
                                            E(c.marker, c.startMarker), b); this.addMarker("end", E(c.marker, c.endMarker), b)
                                }, destroy: function () { this.graphics && (p(this.graphics, function (a) { a.destroy() }), delete this.graphics) }
                            }; b.prototype = {
                                algorithms: A, init: function (a) { this.chart = a; this.connections = []; m(a, "redraw", function () { this.pathfinder.update() }) }, update: function (a) {
                                    var d = this.chart, b = this, e = b.connections; b.connections = []; d.series.forEach(function (a) {
                                        a.visible && !a.options.isInternal && a.points.forEach(function (a) {
                                            var h, e = a.options && a.options.connect &&
                                                w(a.options.connect); a.visible && !1 !== a.isInside && e && e.forEach(function (e) { h = d.get("string" === typeof e ? e : e.to); h instanceof z && h.series.visible && h.visible && !1 !== h.isInside && b.connections.push(new k(a, h, "string" === typeof e ? {} : e)) })
                                        })
                                    }); for (var f = 0, c, h, n = e.length, g = b.connections.length; f < n; ++f) { h = !1; for (c = 0; c < g; ++c)if (e[f].fromPoint === b.connections[c].fromPoint && e[f].toPoint === b.connections[c].toPoint) { b.connections[c].graphics = e[f].graphics; h = !0; break } h || e[f].destroy() } delete this.chartObstacles; delete this.lineObstacles;
                                    b.renderConnections(a)
                                }, renderConnections: function (a) { a ? this.chart.series.forEach(function (a) { var d = function () { var d = a.chart.pathfinder; (d && d.connections || []).forEach(function (d) { d.fromPoint && d.fromPoint.series === a && d.render() }); a.pathfinderRemoveRenderEvent && (a.pathfinderRemoveRenderEvent(), delete a.pathfinderRemoveRenderEvent) }; !1 === a.options.animation ? d() : a.pathfinderRemoveRenderEvent = m(a, "afterAnimate", d) }) : this.connections.forEach(function (a) { a.render() }) }, getChartObstacles: function (b) {
                                    for (var d =
                                        [], e = this.chart.series, f = a(b.algorithmMargin, 0), c, r = 0, h = e.length; r < h; ++r)if (e[r].visible && !e[r].options.isInternal) for (var n = 0, g = e[r].points.length, w; n < g; ++n)w = e[r].points[n], w.visible && (w = D(w)) && d.push({ xMin: w.xMin - f, xMax: w.xMax + f, yMin: w.yMin - f, yMax: w.yMax + f }); d = d.sort(function (a, d) { return a.xMin - d.xMin }); t(b.algorithmMargin) || (c = b.algorithmMargin = y(d), d.forEach(function (a) { a.xMin -= c; a.xMax += c; a.yMin -= c; a.yMax += c })); return d
                                }, getObstacleMetrics: function (a) {
                                    for (var d = 0, b = 0, e, f, c = a.length; c--;)e = a[c].xMax -
                                        a[c].xMin, f = a[c].yMax - a[c].yMin, d < e && (d = e), b < f && (b = f); return { maxHeight: b, maxWidth: d }
                                }, getAlgorithmStartDirection: function (a) { var d = "top" !== a.verticalAlign && "bottom" !== a.verticalAlign; return "left" !== a.align && "right" !== a.align ? d ? void 0 : !1 : d ? !0 : void 0 }
                            }; l.Connection = k; l.Pathfinder = b; I(z.prototype, {
                                getPathfinderAnchorPoint: function (a) {
                                    var d = D(this); switch (a.align) { case "right": var b = "xMax"; break; case "left": b = "xMin" }switch (a.verticalAlign) { case "top": var e = "yMin"; break; case "bottom": e = "yMax" }return {
                                        x: b ?
                                            d[b] : (d.xMin + d.xMax) / 2, y: e ? d[e] : (d.yMin + d.yMax) / 2
                                    }
                                }, getRadiansToVector: function (a, d) { var b; t(d) || (b = D(this)) && (d = { x: (b.xMin + b.xMax) / 2, y: (b.yMin + b.yMax) / 2 }); return Math.atan2(d.y - a.y, a.x - d.x) }, getMarkerVector: function (a, d, b) {
                                    var e = 2 * Math.PI, f = D(this), c = f.xMax - f.xMin, h = f.yMax - f.yMin, n = Math.atan2(h, c), g = !1; c /= 2; var w = h / 2, q = f.xMin + c; f = f.yMin + w; for (var k = q, v = f, p = {}, m = 1, t = 1; a < -Math.PI;)a += e; for (; a > Math.PI;)a -= e; e = Math.tan(a); a > -n && a <= n ? (t = -1, g = !0) : a > n && a <= Math.PI - n ? t = -1 : a > Math.PI - n || a <= -(Math.PI - n) ? (m =
                                        -1, g = !0) : m = -1; g ? (k += m * c, v += t * c * e) : (k += h / (2 * e) * m, v += t * w); b.x !== q && (k = b.x); b.y !== f && (v = b.y); p.x = k + d * Math.cos(a); p.y = v - d * Math.sin(a); return p
                                }
                            }); c.prototype.callbacks.push(function (a) { !1 !== a.options.connectors.enabled && (g(a), this.pathfinder = new b(this), this.pathfinder.update(!0)) })
                    }); J(c, "modules/xrange.src.js", [c["parts/Axis.js"], c["parts/Globals.js"], c["parts/Color.js"], c["parts/Point.js"], c["parts/Utilities.js"]], function (c, l, u, z, x) {
                        var A = u.parse; u = x.addEvent; var D = x.clamp, y = x.correctFloat, k = x.defined,
                            b = x.find, g = x.isNumber, m = x.isObject, t = x.merge, H = x.pick; x = x.seriesType; var I = l.seriesTypes.column, E = l.seriesTypes, p = l.Series; x("xrange", "column", {
                                colorByPoint: !0, dataLabels: { formatter: function () { var a = this.point.partialFill; m(a) && (a = a.amount); if (g(a) && 0 < a) return y(100 * a) + "%" }, inside: !0, verticalAlign: "middle" }, tooltip: { headerFormat: '<span style="font-size: 10px">{point.x} - {point.x2}</span><br/>', pointFormat: '<span style="color:{point.color}">\u25cf</span> {series.name}: <b>{point.yCategory}</b><br/>' },
                                borderRadius: 3, pointRange: 0
                            }, {
                                type: "xrange", parallelArrays: ["x", "x2", "y"], requireSorting: !1, animate: E.line.prototype.animate, cropShoulder: 1, getExtremesFromAll: !0, autoIncrement: l.noop, buildKDTree: l.noop, init: function () { E.column.prototype.init.apply(this, arguments); this.options.stacking = void 0 }, getColumnMetrics: function () { function a() { b.series.forEach(function (a) { var b = a.xAxis; a.xAxis = a.yAxis; a.yAxis = b }) } var b = this.chart; a(); var e = I.prototype.getColumnMetrics.call(this); a(); return e }, cropData: function (a,
                                    b, e, c) { b = p.prototype.cropData.call(this, this.x2Data, b, e, c); b.xData = a.slice(b.start, b.end); return b }, findPointIndex: function (a) { var c = this.cropped, e = this.cropStart, k = this.points, q = a.id; if (q) var f = (f = b(k, function (a) { return a.id === q })) ? f.index : void 0; "undefined" === typeof f && (f = (f = b(k, function (d) { return d.x === a.x && d.x2 === a.x2 && !d.touched })) ? f.index : void 0); c && g(f) && g(e) && f >= e && (f -= e); return f }, translatePoint: function (a) {
                                        var b = this.xAxis, e = this.yAxis, c = this.columnMetrics, q = this.options, f = q.minPointLength ||
                                            0, d = a.plotX, v = H(a.x2, a.x + (a.len || 0)), p = b.translate(v, 0, 0, 0, 1); v = Math.abs(p - d); var B = this.chart.inverted, r = H(q.borderWidth, 1) % 2 / 2, h = c.offset, n = Math.round(c.width); f && (f -= v, 0 > f && (f = 0), d -= f / 2, p += f / 2); d = Math.max(d, -10); p = D(p, -10, b.len + 10); k(a.options.pointWidth) && (h -= (Math.ceil(a.options.pointWidth) - n) / 2, n = Math.ceil(a.options.pointWidth)); q.pointPlacement && g(a.plotY) && e.categories && (a.plotY = e.translate(a.y, 0, 1, 0, 1, q.pointPlacement)); a.shapeArgs = {
                                                x: Math.floor(Math.min(d, p)) + r, y: Math.floor(a.plotY + h) + r,
                                                width: Math.round(Math.abs(p - d)), height: n, r: this.options.borderRadius
                                            }; q = a.shapeArgs.x; f = q + a.shapeArgs.width; 0 > q || f > b.len ? (q = D(q, 0, b.len), f = D(f, 0, b.len), p = f - q, a.dlBox = t(a.shapeArgs, { x: q, width: f - q, centerX: p ? p / 2 : null })) : a.dlBox = null; q = a.tooltipPos; f = B ? 1 : 0; p = B ? 0 : 1; c = this.columnMetrics ? this.columnMetrics.offset : -c.width / 2; q[f] = D(q[f] + v / 2 * (b.reversed ? -1 : 1) * (B ? -1 : 1), 0, b.len - 1); q[p] = D(q[p] + (B ? -1 : 1) * c, 0, e.len - 1); if (c = a.partialFill) m(c) && (c = c.amount), g(c) || (c = 0), e = a.shapeArgs, a.partShapeArgs = {
                                                x: e.x, y: e.y, width: e.width,
                                                height: e.height, r: this.options.borderRadius
                                            }, d = Math.max(Math.round(v * c + a.plotX - d), 0), a.clipRectArgs = { x: b.reversed ? e.x + v - d : e.x, y: e.y, width: d, height: e.height }
                                    }, translate: function () { I.prototype.translate.apply(this, arguments); this.points.forEach(function (a) { this.translatePoint(a) }, this) }, drawPoint: function (a, b) {
                                        var e = this.options, c = this.chart.renderer, g = a.graphic, f = a.shapeType, d = a.shapeArgs, w = a.partShapeArgs, k = a.clipRectArgs, p = a.partialFill, r = e.stacking && !e.borderRadius, h = a.state, n = e.states[h || "normal"] ||
                                            {}, F = "undefined" === typeof h ? "attr" : b; h = this.pointAttribs(a, h); n = H(this.chart.options.chart.animation, n.animation); if (a.isNull || !1 === a.visible) g && (a.graphic = g.destroy()); else {
                                                if (g) g.rect[b](d); else a.graphic = g = c.g("point").addClass(a.getClassName()).add(a.group || this.group), g.rect = c[f](t(d)).addClass(a.getClassName()).addClass("highcharts-partfill-original").add(g); w && (g.partRect ? (g.partRect[b](t(w)), g.partialClipRect[b](t(k))) : (g.partialClipRect = c.clipRect(k.x, k.y, k.width, k.height), g.partRect = c[f](w).addClass("highcharts-partfill-overlay").add(g).clip(g.partialClipRect)));
                                                this.chart.styledMode || (g.rect[b](h, n).shadow(e.shadow, null, r), w && (m(p) || (p = {}), m(e.partialFill) && (p = t(p, e.partialFill)), a = p.fill || A(h.fill).brighten(-.3).get() || A(a.color || this.color).brighten(-.3).get(), h.fill = a, g.partRect[F](h, n).shadow(e.shadow, null, r)))
                                            }
                                    }, drawPoints: function () { var a = this, b = a.getAnimationVerb(); a.points.forEach(function (e) { a.drawPoint(e, b) }) }, getAnimationVerb: function () { return this.chart.pointCount < (this.options.animationLimit || 250) ? "animate" : "attr" }
                            }, {
                                resolveColor: function () {
                                    var a =
                                        this.series; if (a.options.colorByPoint && !this.options.color) { var b = a.options.colors || a.chart.options.colors; var e = this.y % (b ? b.length : a.chart.options.chart.colorCount); b = b && b[e]; a.chart.styledMode || (this.color = b); this.options.colorIndex || (this.colorIndex = e) } else this.color || (this.color = a.color)
                                }, init: function () { z.prototype.init.apply(this, arguments); this.y || (this.y = 0); return this }, setState: function () { z.prototype.setState.apply(this, arguments); this.series.drawPoint(this, this.series.getAnimationVerb()) },
                                getLabelConfig: function () { var a = z.prototype.getLabelConfig.call(this), b = this.series.yAxis.categories; a.x2 = this.x2; a.yCategory = this.yCategory = b && b[this.y]; return a }, tooltipDateKeys: ["x", "x2"], isValid: function () { return "number" === typeof this.x && "number" === typeof this.x2 }
                            }); u(c, "afterGetSeriesExtremes", function () { var a = this.series, b; if (this.isXAxis) { var e = H(this.dataMax, -Number.MAX_VALUE); a.forEach(function (a) { a.x2Data && a.x2Data.forEach(function (a) { a > e && (e = a, b = !0) }) }); b && (this.dataMax = e) } }); ""
                    }); J(c,
                        "parts-gantt/GanttSeries.js", [c["parts/Globals.js"], c["parts/Options.js"], c["parts/Utilities.js"]], function (c, l, u) {
                            var z = l.dateFormat, x = u.isNumber, A = u.merge, D = u.pick; l = u.seriesType; var y = u.splat, k = c.seriesTypes.xrange; l("gantt", "xrange", {
                                grouping: !1, dataLabels: { enabled: !0 }, tooltip: {
                                    headerFormat: '<span style="font-size: 10px">{series.name}</span><br/>', pointFormat: null, pointFormatter: function () {
                                        var b = this.series, c = b.chart.tooltip, k = b.xAxis, t = b.tooltipOptions.dateTimeLabelFormats, l = k.options.startOfWeek,
                                        x = b.tooltipOptions, u = x.xDateFormat; b = this.options.milestone; var p = "<b>" + (this.name || this.yCategory) + "</b>"; if (x.pointFormat) return this.tooltipFormatter(x.pointFormat); u || (u = y(c.getDateFormat(k.closestPointRange, this.start, l, t))[0]); c = z(u, this.start); k = z(u, this.end); p += "<br/>"; return b ? p + (c + "<br/>") : p + ("Start: " + c + "<br/>End: ") + (k + "<br/>")
                                    }
                                }, connectors: {
                                    type: "simpleConnect", animation: { reversed: !0 }, startMarker: { enabled: !0, symbol: "arrow-filled", radius: 4, fill: "#fa0", align: "left" }, endMarker: {
                                        enabled: !1,
                                        align: "right"
                                    }
                                }
                            }, {
                                pointArrayMap: ["start", "end", "y"], keyboardMoveVertical: !1, translatePoint: function (b) { k.prototype.translatePoint.call(this, b); if (b.options.milestone) { var c = b.shapeArgs; var m = c.height; b.shapeArgs = { x: c.x - m / 2, y: c.y, width: m, height: m } } }, drawPoint: function (b, c) {
                                    var g = this.options, t = this.chart.renderer, l = b.shapeArgs, u = b.plotY, y = b.graphic, p = b.selected && "select", a = g.stacking && !g.borderRadius; if (b.options.milestone) if (x(u) && null !== b.y && !1 !== b.visible) {
                                        l = t.symbols.diamond(l.x, l.y, l.width, l.height);
                                        if (y) y[c]({ d: l }); else b.graphic = t.path(l).addClass(b.getClassName(), !0).add(b.group || this.group); this.chart.styledMode || b.graphic.attr(this.pointAttribs(b, p)).shadow(g.shadow, null, a)
                                    } else y && (b.graphic = y.destroy()); else k.prototype.drawPoint.call(this, b, c)
                                }, setData: c.Series.prototype.setData, setGanttPointAliases: function (b) { function c(c, g) { "undefined" !== typeof g && (b[c] = g) } c("x", D(b.start, b.x)); c("x2", D(b.end, b.x2)); c("partialFill", D(b.completed, b.partialFill)); c("connect", D(b.dependency, b.connect)) }
                            },
                                A(k.prototype.pointClass.prototype, { applyOptions: function (b, g) { b = A(b); c.seriesTypes.gantt.prototype.setGanttPointAliases(b); return b = k.prototype.pointClass.prototype.applyOptions.call(this, b, g) }, isValid: function () { return ("number" === typeof this.start || "number" === typeof this.x) && ("number" === typeof this.end || "number" === typeof this.x2 || this.milestone) } })); ""
                        }); J(c, "parts-gantt/GanttChart.js", [c["parts/Chart.js"], c["parts/Globals.js"], c["parts/Utilities.js"]], function (c, l, u) {
                            var z = u.getOptions, x = u.isArray,
                            A = u.merge, D = u.splat; l.ganttChart = function (u, k, b) {
                                var g = "string" === typeof u || u.nodeName, m = k.series, t = z(), y, I = k; k = arguments[g ? 1 : 0]; x(k.xAxis) || (k.xAxis = [k.xAxis || {}, {}]); k.xAxis = k.xAxis.map(function (b, c) { 1 === c && (y = 0); return A(t.xAxis, { grid: { enabled: !0 }, opposite: !0, linkedTo: y }, b, { type: "datetime" }) }); k.yAxis = D(k.yAxis || {}).map(function (b) { return A(t.yAxis, { grid: { enabled: !0 }, staticScale: 50, reversed: !0, type: b.categories ? b.type : "treegrid" }, b) }); k.series = null; k = A(!0, {
                                    chart: { type: "gantt" }, title: { text: null },
                                    legend: { enabled: !1 }
                                }, k, { isGantt: !0 }); k.series = I.series = m; (k.series || []).forEach(function (b) { b.data && b.data.forEach(function (b) { l.seriesTypes.gantt.prototype.setGanttPointAliases(b) }) }); return g ? new c(u, k, b) : new c(k, k)
                            }
                        }); J(c, "parts/ScrollbarAxis.js", [c["parts/Globals.js"], c["parts/Utilities.js"]], function (c, l) {
                            var u = l.addEvent, z = l.defined, x = l.pick; return function () {
                                function l() { } l.compose = function (l, y) {
                                    u(l, "afterInit", function () {
                                        var k = this; k.options && k.options.scrollbar && k.options.scrollbar.enabled &&
                                            (k.options.scrollbar.vertical = !k.horiz, k.options.startOnTick = k.options.endOnTick = !1, k.scrollbar = new y(k.chart.renderer, k.options.scrollbar, k.chart), u(k.scrollbar, "changed", function (b) {
                                                var g = x(k.options && k.options.min, k.min), m = x(k.options && k.options.max, k.max), l = z(k.dataMin) ? Math.min(g, k.min, k.dataMin) : g, u = (z(k.dataMax) ? Math.max(m, k.max, k.dataMax) : m) - l; z(g) && z(m) && (k.horiz && !k.reversed || !k.horiz && k.reversed ? (g = l + u * this.to, l += u * this.from) : (g = l + u * (1 - this.from), l += u * (1 - this.to)), x(this.options.liveRedraw,
                                                    c.svg && !c.isTouchDevice && !this.chart.isBoosting) || "mouseup" === b.DOMType || !z(b.DOMType) ? k.setExtremes(l, g, !0, "mousemove" !== b.DOMType, b) : this.setRange(this.from, this.to))
                                            }))
                                    }); u(l, "afterRender", function () {
                                        var c = Math.min(x(this.options.min, this.min), this.min, x(this.dataMin, this.min)), b = Math.max(x(this.options.max, this.max), this.max, x(this.dataMax, this.max)), g = this.scrollbar, m = this.axisTitleMargin + (this.titleOffset || 0), l = this.chart.scrollbarsOffsets, u = this.options.margin || 0; g && (this.horiz ? (this.opposite ||
                                            (l[1] += m), g.position(this.left, this.top + this.height + 2 + l[1] - (this.opposite ? u : 0), this.width, this.height), this.opposite || (l[1] += u), m = 1) : (this.opposite && (l[0] += m), g.position(this.left + this.width + 2 + l[0] - (this.opposite ? 0 : u), this.top, this.width, this.height), this.opposite && (l[0] += u), m = 0), l[m] += g.size + g.options.margin, isNaN(c) || isNaN(b) || !z(this.min) || !z(this.max) || this.min === this.max ? g.setRange(0, 1) : (l = (this.min - c) / (b - c), c = (this.max - c) / (b - c), this.horiz && !this.reversed || !this.horiz && this.reversed ? g.setRange(l,
                                                c) : g.setRange(1 - c, 1 - l)))
                                    }); u(l, "afterGetOffset", function () { var c = this.horiz ? 2 : 1, b = this.scrollbar; b && (this.chart.scrollbarsOffsets = [0, 0], this.chart.axisOffset[c] += b.size + b.options.margin) })
                                }; return l
                            }()
                        }); J(c, "parts/Scrollbar.js", [c["parts/Axis.js"], c["parts/Globals.js"], c["parts/ScrollbarAxis.js"], c["parts/Utilities.js"], c["parts/Options.js"]], function (c, l, u, z, x) {
                            var A = z.addEvent, D = z.correctFloat, y = z.defined, k = z.destroyObjectProperties, b = z.fireEvent, g = z.merge, m = z.pick, t = z.removeEvent; z = x.defaultOptions;
                            var H = l.hasTouch, I = l.isTouchDevice, E = l.swapXY = function (b, a) { a && b.forEach(function (a) { for (var b = a.length, c, g = 0; g < b; g += 2)c = a[g + 1], "number" === typeof c && (a[g + 1] = a[g + 2], a[g + 2] = c) }); return b }; x = function () {
                                function c(a, b, c) {
                                    this._events = []; this.from = this.chartY = this.chartX = 0; this.scrollbar = this.group = void 0; this.scrollbarButtons = []; this.scrollbarGroup = void 0; this.scrollbarLeft = 0; this.scrollbarRifles = void 0; this.scrollbarStrokeWidth = 1; this.to = this.size = this.scrollbarTop = 0; this.track = void 0; this.trackBorderWidth =
                                        1; this.userOptions = {}; this.y = this.x = 0; this.chart = c; this.options = b; this.renderer = c.renderer; this.init(a, b, c)
                                } c.prototype.addEvents = function () {
                                    var a = this.options.inverted ? [1, 0] : [0, 1], b = this.scrollbarButtons, c = this.scrollbarGroup.element, g = this.track.element, k = this.mouseDownHandler.bind(this), f = this.mouseMoveHandler.bind(this), d = this.mouseUpHandler.bind(this); a = [[b[a[0]].element, "click", this.buttonToMinClick.bind(this)], [b[a[1]].element, "click", this.buttonToMaxClick.bind(this)], [g, "click", this.trackClick.bind(this)],
                                    [c, "mousedown", k], [c.ownerDocument, "mousemove", f], [c.ownerDocument, "mouseup", d]]; H && a.push([c, "touchstart", k], [c.ownerDocument, "touchmove", f], [c.ownerDocument, "touchend", d]); a.forEach(function (a) { A.apply(null, a) }); this._events = a
                                }; c.prototype.buttonToMaxClick = function (a) { var c = (this.to - this.from) * m(this.options.step, .2); this.updatePosition(this.from + c, this.to + c); b(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMEvent: a }) }; c.prototype.buttonToMinClick = function (a) {
                                    var c = D(this.to - this.from) *
                                        m(this.options.step, .2); this.updatePosition(D(this.from - c), D(this.to - c)); b(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMEvent: a })
                                }; c.prototype.cursorToScrollbarPosition = function (a) { var b = this.options; b = b.minWidth > this.calculatedWidth ? b.minWidth : 0; return { chartX: (a.chartX - this.x - this.xOffset) / (this.barWidth - b), chartY: (a.chartY - this.y - this.yOffset) / (this.barWidth - b) } }; c.prototype.destroy = function () {
                                    var a = this.chart.scroller; this.removeEvents();["track", "scrollbarRifles", "scrollbar",
                                        "scrollbarGroup", "group"].forEach(function (a) { this[a] && this[a].destroy && (this[a] = this[a].destroy()) }, this); a && this === a.scrollbar && (a.scrollbar = null, k(a.scrollbarButtons))
                                }; c.prototype.drawScrollbarButton = function (a) {
                                    var b = this.renderer, c = this.scrollbarButtons, g = this.options, k = this.size; var f = b.g().add(this.group); c.push(f); f = b.rect().addClass("highcharts-scrollbar-button").add(f); this.chart.styledMode || f.attr({ stroke: g.buttonBorderColor, "stroke-width": g.buttonBorderWidth, fill: g.buttonBackgroundColor });
                                    f.attr(f.crisp({ x: -.5, y: -.5, width: k + 1, height: k + 1, r: g.buttonBorderRadius }, f.strokeWidth())); f = b.path(E([["M", k / 2 + (a ? -1 : 1), k / 2 - 3], ["L", k / 2 + (a ? -1 : 1), k / 2 + 3], ["L", k / 2 + (a ? 2 : -2), k / 2]], g.vertical)).addClass("highcharts-scrollbar-arrow").add(c[a]); this.chart.styledMode || f.attr({ fill: g.buttonArrowColor })
                                }; c.prototype.init = function (a, b, e) {
                                    this.scrollbarButtons = []; this.renderer = a; this.userOptions = b; this.options = g(c.defaultOptions, b); this.chart = e; this.size = m(this.options.size, this.options.height); b.enabled && (this.render(),
                                        this.addEvents())
                                }; c.prototype.mouseDownHandler = function (a) { a = this.chart.pointer.normalize(a); a = this.cursorToScrollbarPosition(a); this.chartX = a.chartX; this.chartY = a.chartY; this.initPositions = [this.from, this.to]; this.grabbedCenter = !0 }; c.prototype.mouseMoveHandler = function (a) {
                                    var c = this.chart.pointer.normalize(a), e = this.options.vertical ? "chartY" : "chartX", g = this.initPositions || []; !this.grabbedCenter || a.touches && 0 === a.touches[0][e] || (c = this.cursorToScrollbarPosition(c)[e], e = this[e], e = c - e, this.hasDragged =
                                        !0, this.updatePosition(g[0] + e, g[1] + e), this.hasDragged && b(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMType: a.type, DOMEvent: a }))
                                }; c.prototype.mouseUpHandler = function (a) { this.hasDragged && b(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMType: a.type, DOMEvent: a }); this.grabbedCenter = this.hasDragged = this.chartX = this.chartY = null }; c.prototype.position = function (a, b, c, g) {
                                    var e = this.options.vertical, f = 0, d = this.rendered ? "animate" : "attr"; this.x = a; this.y = b + this.trackBorderWidth;
                                    this.width = c; this.xOffset = this.height = g; this.yOffset = f; e ? (this.width = this.yOffset = c = f = this.size, this.xOffset = b = 0, this.barWidth = g - 2 * c, this.x = a += this.options.margin) : (this.height = this.xOffset = g = b = this.size, this.barWidth = c - 2 * g, this.y += this.options.margin); this.group[d]({ translateX: a, translateY: this.y }); this.track[d]({ width: c, height: g }); this.scrollbarButtons[1][d]({ translateX: e ? 0 : c - b, translateY: e ? g - f : 0 })
                                }; c.prototype.removeEvents = function () {
                                    this._events.forEach(function (a) { t.apply(null, a) }); this._events.length =
                                        0
                                }; c.prototype.render = function () {
                                    var a = this.renderer, b = this.options, c = this.size, g = this.chart.styledMode, k; this.group = k = a.g("scrollbar").attr({ zIndex: b.zIndex, translateY: -99999 }).add(); this.track = a.rect().addClass("highcharts-scrollbar-track").attr({ x: 0, r: b.trackBorderRadius || 0, height: c, width: c }).add(k); g || this.track.attr({ fill: b.trackBackgroundColor, stroke: b.trackBorderColor, "stroke-width": b.trackBorderWidth }); this.trackBorderWidth = this.track.strokeWidth(); this.track.attr({
                                        y: -this.trackBorderWidth %
                                            2 / 2
                                    }); this.scrollbarGroup = a.g().add(k); this.scrollbar = a.rect().addClass("highcharts-scrollbar-thumb").attr({ height: c, width: c, r: b.barBorderRadius || 0 }).add(this.scrollbarGroup); this.scrollbarRifles = a.path(E([["M", -3, c / 4], ["L", -3, 2 * c / 3], ["M", 0, c / 4], ["L", 0, 2 * c / 3], ["M", 3, c / 4], ["L", 3, 2 * c / 3]], b.vertical)).addClass("highcharts-scrollbar-rifles").add(this.scrollbarGroup); g || (this.scrollbar.attr({ fill: b.barBackgroundColor, stroke: b.barBorderColor, "stroke-width": b.barBorderWidth }), this.scrollbarRifles.attr({
                                        stroke: b.rifleColor,
                                        "stroke-width": 1
                                    })); this.scrollbarStrokeWidth = this.scrollbar.strokeWidth(); this.scrollbarGroup.translate(-this.scrollbarStrokeWidth % 2 / 2, -this.scrollbarStrokeWidth % 2 / 2); this.drawScrollbarButton(0); this.drawScrollbarButton(1)
                                }; c.prototype.setRange = function (a, b) {
                                    var c = this.options, g = c.vertical, k = c.minWidth, f = this.barWidth, d, v = !this.rendered || this.hasDragged || this.chart.navigator && this.chart.navigator.hasDragged ? "attr" : "animate"; if (y(f)) {
                                        a = Math.max(a, 0); var l = Math.ceil(f * a); this.calculatedWidth = d = D(f *
                                            Math.min(b, 1) - l); d < k && (l = (f - k + d) * a, d = k); k = Math.floor(l + this.xOffset + this.yOffset); f = d / 2 - .5; this.from = a; this.to = b; g ? (this.scrollbarGroup[v]({ translateY: k }), this.scrollbar[v]({ height: d }), this.scrollbarRifles[v]({ translateY: f }), this.scrollbarTop = k, this.scrollbarLeft = 0) : (this.scrollbarGroup[v]({ translateX: k }), this.scrollbar[v]({ width: d }), this.scrollbarRifles[v]({ translateX: f }), this.scrollbarLeft = k, this.scrollbarTop = 0); 12 >= d ? this.scrollbarRifles.hide() : this.scrollbarRifles.show(!0); !1 === c.showFull && (0 >=
                                                a && 1 <= b ? this.group.hide() : this.group.show()); this.rendered = !0
                                    }
                                }; c.prototype.trackClick = function (a) { var c = this.chart.pointer.normalize(a), e = this.to - this.from, g = this.y + this.scrollbarTop, k = this.x + this.scrollbarLeft; this.options.vertical && c.chartY > g || !this.options.vertical && c.chartX > k ? this.updatePosition(this.from + e, this.to + e) : this.updatePosition(this.from - e, this.to - e); b(this, "changed", { from: this.from, to: this.to, trigger: "scrollbar", DOMEvent: a }) }; c.prototype.update = function (a) {
                                    this.destroy(); this.init(this.chart.renderer,
                                        g(!0, this.options, a), this.chart)
                                }; c.prototype.updatePosition = function (a, b) { 1 < b && (a = D(1 - D(b - a)), b = 1); 0 > a && (b = D(b - a), a = 0); this.from = a; this.to = b }; c.defaultOptions = {
                                    height: I ? 20 : 14, barBorderRadius: 0, buttonBorderRadius: 0, liveRedraw: void 0, margin: 10, minWidth: 6, step: .2, zIndex: 3, barBackgroundColor: "#cccccc", barBorderWidth: 1, barBorderColor: "#cccccc", buttonArrowColor: "#333333", buttonBackgroundColor: "#e6e6e6", buttonBorderColor: "#cccccc", buttonBorderWidth: 1, rifleColor: "#333333", trackBackgroundColor: "#f2f2f2",
                                    trackBorderColor: "#f2f2f2", trackBorderWidth: 1
                                }; return c
                            }(); l.Scrollbar || (z.scrollbar = g(!0, x.defaultOptions, z.scrollbar), l.Scrollbar = x, u.compose(c, x)); return l.Scrollbar
                        }); J(c, "parts/RangeSelector.js", [c["parts/Axis.js"], c["parts/Chart.js"], c["parts/Globals.js"], c["parts/Options.js"], c["parts/SVGElement.js"], c["parts/Utilities.js"]], function (c, l, u, z, x, A) {
                            var D = z.defaultOptions, y = A.addEvent, k = A.createElement, b = A.css, g = A.defined, m = A.destroyObjectProperties, t = A.discardElement, H = A.extend, I = A.fireEvent,
                            E = A.isNumber, p = A.merge, a = A.objectEach, w = A.pick, e = A.pInt, C = A.splat; H(D, { rangeSelector: { verticalAlign: "top", buttonTheme: { width: 28, height: 18, padding: 2, zIndex: 7 }, floating: !1, x: 0, y: 0, height: void 0, inputPosition: { align: "right", x: 0, y: 0 }, buttonPosition: { align: "left", x: 0, y: 0 }, labelStyle: { color: "#666666" } } }); D.lang = p(D.lang, { rangeSelectorZoom: "Zoom", rangeSelectorFrom: "From", rangeSelectorTo: "To" }); var q = function () {
                                function f(a) {
                                    this.buttons = void 0; this.buttonOptions = f.prototype.defaultButtons; this.options = void 0;
                                    this.chart = a; this.init(a)
                                } f.prototype.clickButton = function (a, b) {
                                    var d = this.chart, e = this.buttonOptions[a], f = d.xAxis[0], h = d.scroller && d.scroller.getUnionExtremes() || f || {}, n = h.dataMin, g = h.dataMax, k = f && Math.round(Math.min(f.max, w(g, f.max))), v = e.type; h = e._range; var l, p = e.dataGrouping; if (null !== n && null !== g) {
                                        d.fixedRange = h; p && (this.forcedDataGrouping = !0, c.prototype.setDataGrouping.call(f || { chart: this.chart }, p, !1), this.frozenStates = e.preserveDataGrouping); if ("month" === v || "year" === v) if (f) {
                                            v = {
                                                range: e, max: k,
                                                chart: d, dataMin: n, dataMax: g
                                            }; var m = f.minFromRange.call(v); E(v.newMax) && (k = v.newMax)
                                        } else h = e; else if (h) m = Math.max(k - h, n), k = Math.min(m + h, g); else if ("ytd" === v) if (f) "undefined" === typeof g && (n = Number.MAX_VALUE, g = Number.MIN_VALUE, d.series.forEach(function (a) { a = a.xData; n = Math.min(a[0], n); g = Math.max(a[a.length - 1], g) }), b = !1), k = this.getYTDExtremes(g, n, d.time.useUTC), m = l = k.min, k = k.max; else { this.deferredYTDClick = a; return } else "all" === v && f && (m = n, k = g); m += e._offsetMin; k += e._offsetMax; this.setSelected(a); if (f) f.setExtremes(m,
                                            k, w(b, 1), null, { trigger: "rangeSelectorButton", rangeSelectorButton: e }); else { var q = C(d.options.xAxis)[0]; var t = q.range; q.range = h; var u = q.min; q.min = l; y(d, "load", function () { q.range = t; q.min = u }) }
                                    }
                                }; f.prototype.setSelected = function (a) { this.selected = this.options.selected = a }; f.prototype.init = function (a) {
                                    var b = this, c = a.options.rangeSelector, d = c.buttons || b.defaultButtons.slice(), e = c.selected, h = function () { var a = b.minInput, c = b.maxInput; a && a.blur && I(a, "blur"); c && c.blur && I(c, "blur") }; b.chart = a; b.options = c; b.buttons =
                                        []; b.buttonOptions = d; this.unMouseDown = y(a.container, "mousedown", h); this.unResize = y(a, "resize", h); d.forEach(b.computeButtonRange); "undefined" !== typeof e && d[e] && this.clickButton(e, !1); y(a, "load", function () { a.xAxis && a.xAxis[0] && y(a.xAxis[0], "setExtremes", function (c) { this.max - this.min !== a.fixedRange && "rangeSelectorButton" !== c.trigger && "updatedData" !== c.trigger && b.forcedDataGrouping && !b.frozenStates && this.setDataGrouping(!1, !1) }) })
                                }; f.prototype.updateButtonStates = function () {
                                    var a = this, b = this.chart, c = b.xAxis[0],
                                    e = Math.round(c.max - c.min), f = !c.hasVisibleSeries, h = b.scroller && b.scroller.getUnionExtremes() || c, n = h.dataMin, g = h.dataMax; b = a.getYTDExtremes(g, n, b.time.useUTC); var k = b.min, l = b.max, p = a.selected, m = E(p), q = a.options.allButtonsEnabled, t = a.buttons; a.buttonOptions.forEach(function (b, d) {
                                        var h = b._range, r = b.type, F = b.count || 1, v = t[d], B = 0, G = b._offsetMax - b._offsetMin; b = d === p; var w = h > g - n, K = h < c.minRange, u = !1, P = !1; h = h === e; ("month" === r || "year" === r) && e + 36E5 >= 864E5 * { month: 28, year: 365 }[r] * F - G && e - 36E5 <= 864E5 * { month: 31, year: 366 }[r] *
                                            F + G ? h = !0 : "ytd" === r ? (h = l - k + G === e, u = !b) : "all" === r && (h = c.max - c.min >= g - n, P = !b && m && h); r = !q && (w || K || P || f); F = b && h || h && !m && !u || b && a.frozenStates; r ? B = 3 : F && (m = !0, B = 2); v.state !== B && (v.setState(B), 0 === B && p === d && a.setSelected(null))
                                    })
                                }; f.prototype.computeButtonRange = function (a) {
                                    var b = a.type, c = a.count || 1, d = { millisecond: 1, second: 1E3, minute: 6E4, hour: 36E5, day: 864E5, week: 6048E5 }; if (d[b]) a._range = d[b] * c; else if ("month" === b || "year" === b) a._range = 864E5 * { month: 30, year: 365 }[b] * c; a._offsetMin = w(a.offsetMin, 0); a._offsetMax =
                                        w(a.offsetMax, 0); a._range += a._offsetMax - a._offsetMin
                                }; f.prototype.setInputValue = function (a, b) { var c = this.chart.options.rangeSelector, d = this.chart.time, e = this[a + "Input"]; g(b) && (e.previousValue = e.HCTime, e.HCTime = b); e.value = d.dateFormat(c.inputEditDateFormat || "%Y-%m-%d", e.HCTime); this[a + "DateBox"].attr({ text: d.dateFormat(c.inputDateFormat || "%b %e, %Y", e.HCTime) }) }; f.prototype.showInput = function (a) {
                                    var c = this.inputGroup, d = this[a + "DateBox"]; b(this[a + "Input"], {
                                        left: c.translateX + d.x + "px", top: c.translateY +
                                            "px", width: d.width - 2 + "px", height: d.height - 2 + "px", border: "2px solid silver"
                                    })
                                }; f.prototype.hideInput = function (a) { b(this[a + "Input"], { border: 0, width: "1px", height: "1px" }); this.setInputValue(a) }; f.prototype.drawInput = function (a) {
                                    function c() {
                                        var a = m.value, b = (n.inputDateParser || Date.parse)(a), c = f.xAxis[0], h = f.scroller && f.scroller.xAxis ? f.scroller.xAxis : c, g = h.dataMin; h = h.dataMax; b !== m.previousValue && (m.previousValue = b, E(b) || (b = a.split("-"), b = Date.UTC(e(b[0]), e(b[1]) - 1, e(b[2]))), E(b) && (f.time.useUTC || (b +=
                                            6E4 * (new Date).getTimezoneOffset()), l ? b > d.maxInput.HCTime ? b = void 0 : b < g && (b = g) : b < d.minInput.HCTime ? b = void 0 : b > h && (b = h), "undefined" !== typeof b && c.setExtremes(l ? b : c.min, l ? c.max : b, void 0, void 0, { trigger: "rangeSelectorInput" })))
                                    } var d = this, f = d.chart, g = f.renderer.style || {}, h = f.renderer, n = f.options.rangeSelector, F = d.div, l = "min" === a, m, q, t = this.inputGroup; this[a + "Label"] = q = h.label(D.lang[l ? "rangeSelectorFrom" : "rangeSelectorTo"], this.inputGroup.offset).addClass("highcharts-range-label").attr({ padding: 2 }).add(t);
                                    t.offset += q.width + 5; this[a + "DateBox"] = h = h.label("", t.offset).addClass("highcharts-range-input").attr({ padding: 2, width: n.inputBoxWidth || 90, height: n.inputBoxHeight || 17, "text-align": "center" }).on("click", function () { d.showInput(a); d[a + "Input"].focus() }); f.styledMode || h.attr({ stroke: n.inputBoxBorderColor || "#cccccc", "stroke-width": 1 }); h.add(t); t.offset += h.width + (l ? 10 : 0); this[a + "Input"] = m = k("input", { name: a, className: "highcharts-range-selector", type: "text" }, { top: f.plotTop + "px" }, F); f.styledMode || (q.css(p(g,
                                        n.labelStyle)), h.css(p({ color: "#333333" }, g, n.inputStyle)), b(m, H({ position: "absolute", border: 0, width: "1px", height: "1px", padding: 0, textAlign: "center", fontSize: g.fontSize, fontFamily: g.fontFamily, top: "-9999em" }, n.inputStyle))); m.onfocus = function () { d.showInput(a) }; m.onblur = function () { m === u.doc.activeElement && c(); d.hideInput(a); m.blur() }; m.onchange = c; m.onkeypress = function (a) { 13 === a.keyCode && c() }
                                }; f.prototype.getPosition = function () {
                                    var a = this.chart, b = a.options.rangeSelector; a = "top" === b.verticalAlign ? a.plotTop -
                                        a.axisOffset[0] : 0; return { buttonTop: a + b.buttonPosition.y, inputTop: a + b.inputPosition.y - 10 }
                                }; f.prototype.getYTDExtremes = function (a, b, c) { var d = this.chart.time, e = new d.Date(a), h = d.get("FullYear", e); c = c ? d.Date.UTC(h, 0, 1) : +new d.Date(h, 0, 1); b = Math.max(b || 0, c); e = e.getTime(); return { max: Math.min(a || e, e), min: b } }; f.prototype.render = function (a, b) {
                                    var c = this, d = c.chart, e = d.renderer, h = d.container, n = d.options, f = n.exporting && !1 !== n.exporting.enabled && n.navigation && n.navigation.buttonOptions, g = D.lang, l = c.div, m = n.rangeSelector,
                                    p = w(n.chart.style && n.chart.style.zIndex, 0) + 1; n = m.floating; var q = c.buttons; l = c.inputGroup; var v = m.buttonTheme, t = m.buttonPosition, u = m.inputPosition, x = m.inputEnabled, y = v && v.states, z = d.plotLeft, A = c.buttonGroup, C, E = c.options.verticalAlign, H = d.legend, I = H && H.options, J = t.y, N = u.y, O = d.hasLoaded, Q = O ? "animate" : "attr", M = 0, L = 0; if (!1 !== m.enabled) {
                                        c.rendered || (c.group = C = e.g("range-selector-group").attr({ zIndex: 7 }).add(), c.buttonGroup = A = e.g("range-selector-buttons").add(C), c.zoomText = e.text(g.rangeSelectorZoom, 0,
                                            15).add(A), d.styledMode || (c.zoomText.css(m.labelStyle), v["stroke-width"] = w(v["stroke-width"], 0)), c.buttonOptions.forEach(function (a, b) { q[b] = e.button(a.text, 0, 0, function (h) { var d = a.events && a.events.click, e; d && (e = d.call(a, h)); !1 !== e && c.clickButton(b); c.isActive = !0 }, v, y && y.hover, y && y.select, y && y.disabled).attr({ "text-align": "center" }).add(A) }), !1 !== x && (c.div = l = k("div", null, { position: "relative", height: 0, zIndex: p }), h.parentNode.insertBefore(l, h), c.inputGroup = l = e.g("input-group").add(C), l.offset = 0, c.drawInput("min"),
                                                c.drawInput("max"))); c.zoomText[Q]({ x: w(z + t.x, z) }); var R = w(z + t.x, z) + c.zoomText.getBBox().width + 5; c.buttonOptions.forEach(function (a, b) { q[b][Q]({ x: R }); R += q[b].width + w(m.buttonSpacing, 5) }); z = d.plotLeft - d.spacing[3]; c.updateButtonStates(); f && this.titleCollision(d) && "top" === E && "right" === t.align && t.y + A.getBBox().height - 12 < (f.y || 0) + f.height && (M = -40); h = t.x - d.spacing[3]; "right" === t.align ? h += M - z : "center" === t.align && (h -= z / 2); A.align({ y: t.y, width: A.getBBox().width, align: t.align, x: h }, !0, d.spacingBox); c.group.placed =
                                                    O; c.buttonGroup.placed = O; !1 !== x && (M = f && this.titleCollision(d) && "top" === E && "right" === u.align && u.y - l.getBBox().height - 12 < (f.y || 0) + f.height + d.spacing[0] ? -40 : 0, "left" === u.align ? h = z : "right" === u.align && (h = -Math.max(d.axisOffset[1], -M)), l.align({ y: u.y, width: l.getBBox().width, align: u.align, x: u.x + h - 2 }, !0, d.spacingBox), f = l.alignAttr.translateX + l.alignOptions.x - M + l.getBBox().x + 2, h = l.alignOptions.width, g = A.alignAttr.translateX + A.getBBox().x, z = A.getBBox().width + 20, (u.align === t.align || g + z > f && f + h > g && J < N + l.getBBox().height) &&
                                                        l.attr({ translateX: l.alignAttr.translateX + (d.axisOffset[1] >= -M ? 0 : -M), translateY: l.alignAttr.translateY + A.getBBox().height + 10 }), c.setInputValue("min", a), c.setInputValue("max", b), c.inputGroup.placed = O); c.group.align({ verticalAlign: E }, !0, d.spacingBox); a = c.group.getBBox().height + 20; b = c.group.alignAttr.translateY; "bottom" === E && (H = I && "bottom" === I.verticalAlign && I.enabled && !I.floating ? H.legendHeight + w(I.margin, 10) : 0, a = a + H - 20, L = b - a - (n ? 0 : m.y) - (d.titleOffset ? d.titleOffset[2] : 0) - 10); if ("top" === E) n && (L = 0), d.titleOffset &&
                                                            d.titleOffset[0] && (L = d.titleOffset[0]), L += d.margin[0] - d.spacing[0] || 0; else if ("middle" === E) if (N === J) L = 0 > N ? b + void 0 : b; else if (N || J) L = 0 > N || 0 > J ? L - Math.min(N, J) : b - a + NaN; c.group.translate(m.x, m.y + Math.floor(L)); !1 !== x && (c.minInput.style.marginTop = c.group.translateY + "px", c.maxInput.style.marginTop = c.group.translateY + "px"); c.rendered = !0
                                    }
                                }; f.prototype.getHeight = function () {
                                    var a = this.options, b = this.group, c = a.y, e = a.buttonPosition.y, f = a.inputPosition.y; if (a.height) return a.height; a = b ? b.getBBox(!0).height + 13 +
                                        c : 0; b = Math.min(f, e); if (0 > f && 0 > e || 0 < f && 0 < e) a += Math.abs(b); return a
                                }; f.prototype.titleCollision = function (a) { return !(a.options.title.text || a.options.subtitle.text) }; f.prototype.update = function (a) { var b = this.chart; p(!0, b.options.rangeSelector, a); this.destroy(); this.init(b); b.rangeSelector.render() }; f.prototype.destroy = function () {
                                    var b = this, c = b.minInput, e = b.maxInput; b.unMouseDown(); b.unResize(); m(b.buttons); c && (c.onfocus = c.onblur = c.onchange = null); e && (e.onfocus = e.onblur = e.onchange = null); a(b, function (a,
                                        c) { a && "chart" !== c && (a instanceof x ? a.destroy() : a instanceof window.HTMLElement && t(a)); a !== f.prototype[c] && (b[c] = null) }, this)
                                }; return f
                            }(); q.prototype.defaultButtons = [{ type: "month", count: 1, text: "1m" }, { type: "month", count: 3, text: "3m" }, { type: "month", count: 6, text: "6m" }, { type: "ytd", text: "YTD" }, { type: "year", count: 1, text: "1y" }, { type: "all", text: "All" }]; c.prototype.minFromRange = function () {
                                var a = this.range, b = a.type, c = this.max, e = this.chart.time, g = function (a, c) {
                                    var h = "year" === b ? "FullYear" : "Month", d = new e.Date(a),
                                    n = e.get(h, d); e.set(h, d, n + c); n === e.get(h, d) && e.set("Date", d, 0); return d.getTime() - a
                                }; if (E(a)) { var k = c - a; var h = a } else k = c + g(c, -a.count), this.chart && (this.chart.fixedRange = c - k); var n = w(this.dataMin, Number.MIN_VALUE); E(k) || (k = n); k <= n && (k = n, "undefined" === typeof h && (h = g(k, a.count)), this.newMax = Math.min(k + h, this.dataMax)); E(c) || (k = void 0); return k
                            }; u.RangeSelector || (y(l, "afterGetContainer", function () { this.options.rangeSelector.enabled && (this.rangeSelector = new q(this)) }), y(l, "beforeRender", function () {
                                var a =
                                    this.axes, b = this.rangeSelector; b && (E(b.deferredYTDClick) && (b.clickButton(b.deferredYTDClick), delete b.deferredYTDClick), a.forEach(function (a) { a.updateNames(); a.setScale() }), this.getAxisMargins(), b.render(), a = b.options.verticalAlign, b.options.floating || ("bottom" === a ? this.extraBottomMargin = !0 : "middle" !== a && (this.extraTopMargin = !0)))
                            }), y(l, "update", function (a) {
                                var b = a.options.rangeSelector; a = this.rangeSelector; var c = this.extraBottomMargin, e = this.extraTopMargin; b && b.enabled && !g(a) && (this.options.rangeSelector.enabled =
                                    !0, this.rangeSelector = new q(this)); this.extraTopMargin = this.extraBottomMargin = !1; a && (a.render(), b = b && b.verticalAlign || a.options && a.options.verticalAlign, a.options.floating || ("bottom" === b ? this.extraBottomMargin = !0 : "middle" !== b && (this.extraTopMargin = !0)), this.extraBottomMargin !== c || this.extraTopMargin !== e) && (this.isDirtyBox = !0)
                            }), y(l, "render", function () {
                                var a = this.rangeSelector; a && !a.options.floating && (a.render(), a = a.options.verticalAlign, "bottom" === a ? this.extraBottomMargin = !0 : "middle" !== a && (this.extraTopMargin =
                                    !0))
                            }), y(l, "getMargins", function () { var a = this.rangeSelector; a && (a = a.getHeight(), this.extraTopMargin && (this.plotTop += a), this.extraBottomMargin && (this.marginBottom += a)) }), l.prototype.callbacks.push(function (a) {
                                function b() { c = a.xAxis[0].getExtremes(); f = a.legend; h = null === e || void 0 === e ? void 0 : e.options.verticalAlign; E(c.min) && e.render(c.min, c.max); e && f.display && "top" === h && h === f.options.verticalAlign && (g = p(a.spacingBox), g.y = "vertical" === f.options.layout ? a.plotTop : g.y + e.getHeight(), f.group.placed = !1, f.align(g)) }
                                var c, e = a.rangeSelector, f, g, h; if (e) { var n = y(a.xAxis[0], "afterSetExtremes", function (a) { e.render(a.min, a.max) }); var k = y(a, "redraw", b); b() } y(a, "destroy", function () { e && (k(), n()) })
                            }), u.RangeSelector = q); return u.RangeSelector
                        }); J(c, "parts/NavigatorAxis.js", [c["parts/Globals.js"], c["parts/Utilities.js"]], function (c, l) {
                            var u = c.isTouchDevice, z = l.addEvent, x = l.correctFloat, A = l.defined, D = l.isNumber, y = l.pick, k = function () {
                                function b(b) { this.axis = b } b.prototype.destroy = function () { this.axis = void 0 }; b.prototype.toFixedRange =
                                    function (b, c, k, l) { var g = this.axis, m = g.chart; m = m && m.fixedRange; var p = (g.pointRange || 0) / 2; b = y(k, g.translate(b, !0, !g.horiz)); c = y(l, g.translate(c, !0, !g.horiz)); g = m && (c - b) / m; A(k) || (b = x(b + p)); A(l) || (c = x(c - p)); .7 < g && 1.3 > g && (l ? b = c - m : c = b + m); D(b) && D(c) || (b = c = void 0); return { min: b, max: c } }; return b
                            }(); return function () {
                                function b() { } b.compose = function (b) {
                                    b.keepProps.push("navigatorAxis"); z(b, "init", function () { this.navigatorAxis || (this.navigatorAxis = new k(this)) }); z(b, "zoom", function (b) {
                                        var c = this.chart.options,
                                        g = c.navigator, k = this.navigatorAxis, l = c.chart.pinchType, m = c.rangeSelector; c = c.chart.zoomType; this.isXAxis && (g && g.enabled || m && m.enabled) && ("y" === c ? b.zoomed = !1 : (!u && "xy" === c || u && "xy" === l) && this.options.range && (g = k.previousZoom, A(b.newMin) ? k.previousZoom = [this.min, this.max] : g && (b.newMin = g[0], b.newMax = g[1], k.previousZoom = void 0))); "undefined" !== typeof b.zoomed && b.preventDefault()
                                    })
                                }; b.AdditionsClass = k; return b
                            }()
                        }); J(c, "parts/Navigator.js", [c["parts/Axis.js"], c["parts/Chart.js"], c["parts/Color.js"], c["parts/Globals.js"],
                        c["parts/NavigatorAxis.js"], c["parts/Options.js"], c["parts/Scrollbar.js"], c["parts/Utilities.js"]], function (c, l, u, z, x, A, D, y) {
                            u = u.parse; var k = A.defaultOptions, b = y.addEvent, g = y.clamp, m = y.correctFloat, t = y.defined, H = y.destroyObjectProperties, I = y.erase, E = y.extend, p = y.find, a = y.isArray, w = y.isNumber, e = y.merge, C = y.pick, q = y.removeEvent, f = y.splat, d = z.hasTouch, v = z.isTouchDevice; A = z.Series; var G = function (a) {
                                for (var b = [], c = 1; c < arguments.length; c++)b[c - 1] = arguments[c]; b = [].filter.call(b, w); if (b.length) return Math[a].apply(0,
                                    b)
                            }; y = "undefined" === typeof z.seriesTypes.areaspline ? "line" : "areaspline"; E(k, {
                                navigator: {
                                    height: 40, margin: 25, maskInside: !0, handles: { width: 7, height: 15, symbols: ["navigator-handle", "navigator-handle"], enabled: !0, lineWidth: 1, backgroundColor: "#f2f2f2", borderColor: "#999999" }, maskFill: u("#6685c2").setOpacity(.3).get(), outlineColor: "#cccccc", outlineWidth: 1, series: {
                                        type: y, fillOpacity: .05, lineWidth: 1, compare: null, dataGrouping: {
                                            approximation: "average", enabled: !0, groupPixelWidth: 2, smoothed: !0, units: [["millisecond",
                                                [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15, 30]], ["hour", [1, 2, 3, 4, 6, 8, 12]], ["day", [1, 2, 3, 4]], ["week", [1, 2, 3]], ["month", [1, 3, 6]], ["year", null]]
                                        }, dataLabels: { enabled: !1, zIndex: 2 }, id: "highcharts-navigator-series", className: "highcharts-navigator-series", lineColor: null, marker: { enabled: !1 }, threshold: null
                                    }, xAxis: {
                                        overscroll: 0, className: "highcharts-navigator-xaxis", tickLength: 0, lineWidth: 0, gridLineColor: "#e6e6e6", gridLineWidth: 1, tickPixelInterval: 200, labels: {
                                            align: "left",
                                            style: { color: "#999999" }, x: 3, y: -4
                                        }, crosshair: !1
                                    }, yAxis: { className: "highcharts-navigator-yaxis", gridLineWidth: 0, startOnTick: !1, endOnTick: !1, minPadding: .1, maxPadding: .1, labels: { enabled: !1 }, crosshair: !1, title: { text: null }, tickLength: 0, tickWidth: 0 }
                                }
                            }); z.Renderer.prototype.symbols["navigator-handle"] = function (a, b, c, e, d) { a = (d && d.width || 0) / 2; b = Math.round(a / 3) + .5; d = d && d.height || 0; return [["M", -a - 1, .5], ["L", a, .5], ["L", a, d + .5], ["L", -a - 1, d + .5], ["L", -a - 1, .5], ["M", -b, 4], ["L", -b, d - 3], ["M", b - 1, 4], ["L", b - 1, d - 3]] }; var B =
                                function () {
                                    function l(a) { this.zoomedMin = this.zoomedMax = this.yAxis = this.xAxis = this.top = this.size = this.shades = this.rendered = this.range = this.outlineHeight = this.outline = this.opposite = this.navigatorSize = this.navigatorSeries = this.navigatorOptions = this.navigatorGroup = this.navigatorEnabled = this.left = this.height = this.handles = this.chart = this.baseSeries = void 0; this.init(a) } l.prototype.drawHandle = function (a, b, c, e) {
                                        var h = this.navigatorOptions.handles.height; this.handles[b][e](c ? {
                                            translateX: Math.round(this.left +
                                                this.height / 2), translateY: Math.round(this.top + parseInt(a, 10) + .5 - h)
                                        } : { translateX: Math.round(this.left + parseInt(a, 10)), translateY: Math.round(this.top + this.height / 2 - h / 2 - 1) })
                                    }; l.prototype.drawOutline = function (a, b, c, e) {
                                        var h = this.navigatorOptions.maskInside, d = this.outline.strokeWidth(), n = d / 2, f = d % 2 / 2; d = this.outlineHeight; var g = this.scrollbarHeight || 0, k = this.size, l = this.left - g, m = this.top; c ? (l -= n, c = m + b + f, b = m + a + f, f = [["M", l + d, m - g - f], ["L", l + d, c], ["L", l, c], ["L", l, b], ["L", l + d, b], ["L", l + d, m + k + g]], h && f.push(["M",
                                            l + d, c - n], ["L", l + d, b + n])) : (a += l + g - f, b += l + g - f, m += n, f = [["M", l, m], ["L", a, m], ["L", a, m + d], ["L", b, m + d], ["L", b, m], ["L", l + k + 2 * g, m]], h && f.push(["M", a - n, m], ["L", b + n, m])); this.outline[e]({ d: f })
                                    }; l.prototype.drawMasks = function (a, b, c, e) { var d = this.left, h = this.top, n = this.height; if (c) { var f = [d, d, d]; var g = [h, h + a, h + b]; var k = [n, n, n]; var l = [a, b - a, this.size - b] } else f = [d, d + a, d + b], g = [h, h, h], k = [a, b - a, this.size - b], l = [n, n, n]; this.shades.forEach(function (a, b) { a[e]({ x: f[b], y: g[b], width: k[b], height: l[b] }) }) }; l.prototype.renderElements =
                                        function () {
                                            var a = this, b = a.navigatorOptions, c = b.maskInside, e = a.chart, d = e.renderer, f, g = { cursor: e.inverted ? "ns-resize" : "ew-resize" }; a.navigatorGroup = f = d.g("navigator").attr({ zIndex: 8, visibility: "hidden" }).add();[!c, c, !c].forEach(function (c, h) { a.shades[h] = d.rect().addClass("highcharts-navigator-mask" + (1 === h ? "-inside" : "-outside")).add(f); e.styledMode || a.shades[h].attr({ fill: c ? b.maskFill : "rgba(0,0,0,0)" }).css(1 === h && g) }); a.outline = d.path().addClass("highcharts-navigator-outline").add(f); e.styledMode || a.outline.attr({
                                                "stroke-width": b.outlineWidth,
                                                stroke: b.outlineColor
                                            }); b.handles.enabled && [0, 1].forEach(function (c) { b.handles.inverted = e.inverted; a.handles[c] = d.symbol(b.handles.symbols[c], -b.handles.width / 2 - 1, 0, b.handles.width, b.handles.height, b.handles); a.handles[c].attr({ zIndex: 7 - c }).addClass("highcharts-navigator-handle highcharts-navigator-handle-" + ["left", "right"][c]).add(f); if (!e.styledMode) { var h = b.handles; a.handles[c].attr({ fill: h.backgroundColor, stroke: h.borderColor, "stroke-width": h.lineWidth }).css(g) } })
                                        }; l.prototype.update = function (a) {
                                            (this.series ||
                                                []).forEach(function (a) { a.baseSeries && delete a.baseSeries.navigatorSeries }); this.destroy(); e(!0, this.chart.options.navigator, this.options, a); this.init(this.chart)
                                        }; l.prototype.render = function (a, b, c, e) {
                                            var d = this.chart, h = this.scrollbarHeight, f, n = this.xAxis, k = n.pointRange || 0; var l = n.navigatorAxis.fake ? d.xAxis[0] : n; var p = this.navigatorEnabled, q, r = this.rendered; var F = d.inverted; var u = d.xAxis[0].minRange, v = d.xAxis[0].options.maxRange; if (!this.hasDragged || t(c)) {
                                                a = m(a - k / 2); b = m(b + k / 2); if (!w(a) || !w(b)) if (r) c =
                                                    0, e = C(n.width, l.width); else return; this.left = C(n.left, d.plotLeft + h + (F ? d.plotWidth : 0)); this.size = q = f = C(n.len, (F ? d.plotHeight : d.plotWidth) - 2 * h); d = F ? h : f + 2 * h; c = C(c, n.toPixels(a, !0)); e = C(e, n.toPixels(b, !0)); w(c) && Infinity !== Math.abs(c) || (c = 0, e = d); a = n.toValue(c, !0); b = n.toValue(e, !0); var x = Math.abs(m(b - a)); x < u ? this.grabbedLeft ? c = n.toPixels(b - u - k, !0) : this.grabbedRight && (e = n.toPixels(a + u + k, !0)) : t(v) && m(x - k) > v && (this.grabbedLeft ? c = n.toPixels(b - v - k, !0) : this.grabbedRight && (e = n.toPixels(a + v + k, !0))); this.zoomedMax =
                                                        g(Math.max(c, e), 0, q); this.zoomedMin = g(this.fixedWidth ? this.zoomedMax - this.fixedWidth : Math.min(c, e), 0, q); this.range = this.zoomedMax - this.zoomedMin; q = Math.round(this.zoomedMax); c = Math.round(this.zoomedMin); p && (this.navigatorGroup.attr({ visibility: "visible" }), r = r && !this.hasDragged ? "animate" : "attr", this.drawMasks(c, q, F, r), this.drawOutline(c, q, F, r), this.navigatorOptions.handles.enabled && (this.drawHandle(c, 0, F, r), this.drawHandle(q, 1, F, r))); this.scrollbar && (F ? (F = this.top - h, l = this.left - h + (p || !l.opposite ? 0 :
                                                            (l.titleOffset || 0) + l.axisTitleMargin), h = f + 2 * h) : (F = this.top + (p ? this.height : -h), l = this.left - h), this.scrollbar.position(l, F, d, h), this.scrollbar.setRange(this.zoomedMin / (f || 1), this.zoomedMax / (f || 1))); this.rendered = !0
                                            }
                                        }; l.prototype.addMouseEvents = function () {
                                            var a = this, c = a.chart, e = c.container, f = [], g, k; a.mouseMoveHandler = g = function (b) { a.onMouseMove(b) }; a.mouseUpHandler = k = function (b) { a.onMouseUp(b) }; f = a.getPartsEvents("mousedown"); f.push(b(c.renderTo, "mousemove", g), b(e.ownerDocument, "mouseup", k)); d && (f.push(b(c.renderTo,
                                                "touchmove", g), b(e.ownerDocument, "touchend", k)), f.concat(a.getPartsEvents("touchstart"))); a.eventsToUnbind = f; a.series && a.series[0] && f.push(b(a.series[0].xAxis, "foundExtremes", function () { c.navigator.modifyNavigatorAxisExtremes() }))
                                        }; l.prototype.getPartsEvents = function (a) { var c = this, e = [];["shades", "handles"].forEach(function (d) { c[d].forEach(function (h, f) { e.push(b(h.element, a, function (a) { c[d + "Mousedown"](a, f) })) }) }); return e }; l.prototype.shadesMousedown = function (a, b) {
                                            a = this.chart.pointer.normalize(a);
                                            var c = this.chart, e = this.xAxis, d = this.zoomedMin, h = this.left, f = this.size, g = this.range, n = a.chartX; c.inverted && (n = a.chartY, h = this.top); if (1 === b) this.grabbedCenter = n, this.fixedWidth = g, this.dragOffset = n - d; else {
                                                a = n - h - g / 2; if (0 === b) a = Math.max(0, a); else if (2 === b && a + g >= f) if (a = f - g, this.reversedExtremes) { a -= g; var k = this.getUnionExtremes().dataMin } else var l = this.getUnionExtremes().dataMax; a !== d && (this.fixedWidth = g, b = e.navigatorAxis.toFixedRange(a, a + g, k, l), t(b.min) && c.xAxis[0].setExtremes(Math.min(b.min, b.max),
                                                    Math.max(b.min, b.max), !0, null, { trigger: "navigator" }))
                                            }
                                        }; l.prototype.handlesMousedown = function (a, b) { this.chart.pointer.normalize(a); a = this.chart; var c = a.xAxis[0], e = this.reversedExtremes; 0 === b ? (this.grabbedLeft = !0, this.otherHandlePos = this.zoomedMax, this.fixedExtreme = e ? c.min : c.max) : (this.grabbedRight = !0, this.otherHandlePos = this.zoomedMin, this.fixedExtreme = e ? c.max : c.min); a.fixedRange = null }; l.prototype.onMouseMove = function (a) {
                                            var b = this, c = b.chart, e = b.left, d = b.navigatorSize, h = b.range, f = b.dragOffset, g = c.inverted;
                                            a.touches && 0 === a.touches[0].pageX || (a = c.pointer.normalize(a), c = a.chartX, g && (e = b.top, c = a.chartY), b.grabbedLeft ? (b.hasDragged = !0, b.render(0, 0, c - e, b.otherHandlePos)) : b.grabbedRight ? (b.hasDragged = !0, b.render(0, 0, b.otherHandlePos, c - e)) : b.grabbedCenter && (b.hasDragged = !0, c < f ? c = f : c > d + f - h && (c = d + f - h), b.render(0, 0, c - f, c - f + h)), b.hasDragged && b.scrollbar && C(b.scrollbar.options.liveRedraw, z.svg && !v && !this.chart.isBoosting) && (a.DOMType = a.type, setTimeout(function () { b.onMouseUp(a) }, 0)))
                                        }; l.prototype.onMouseUp = function (a) {
                                            var b =
                                                this.chart, c = this.xAxis, e = this.scrollbar, d = a.DOMEvent || a, h = b.inverted, f = this.rendered && !this.hasDragged ? "animate" : "attr", g = Math.round(this.zoomedMax), k = Math.round(this.zoomedMin); if (this.hasDragged && (!e || !e.hasDragged) || "scrollbar" === a.trigger) {
                                                    e = this.getUnionExtremes(); if (this.zoomedMin === this.otherHandlePos) var l = this.fixedExtreme; else if (this.zoomedMax === this.otherHandlePos) var m = this.fixedExtreme; this.zoomedMax === this.size && (m = this.reversedExtremes ? e.dataMin : e.dataMax); 0 === this.zoomedMin && (l =
                                                        this.reversedExtremes ? e.dataMax : e.dataMin); c = c.navigatorAxis.toFixedRange(this.zoomedMin, this.zoomedMax, l, m); t(c.min) && b.xAxis[0].setExtremes(Math.min(c.min, c.max), Math.max(c.min, c.max), !0, this.hasDragged ? !1 : null, { trigger: "navigator", triggerOp: "navigator-drag", DOMEvent: d })
                                                } "mousemove" !== a.DOMType && "touchmove" !== a.DOMType && (this.grabbedLeft = this.grabbedRight = this.grabbedCenter = this.fixedWidth = this.fixedExtreme = this.otherHandlePos = this.hasDragged = this.dragOffset = null); this.navigatorEnabled && (this.shades &&
                                                    this.drawMasks(k, g, h, f), this.outline && this.drawOutline(k, g, h, f), this.navigatorOptions.handles.enabled && Object.keys(this.handles).length === this.handles.length && (this.drawHandle(k, 0, h, f), this.drawHandle(g, 1, h, f)))
                                        }; l.prototype.removeEvents = function () { this.eventsToUnbind && (this.eventsToUnbind.forEach(function (a) { a() }), this.eventsToUnbind = void 0); this.removeBaseSeriesEvents() }; l.prototype.removeBaseSeriesEvents = function () {
                                            var a = this.baseSeries || []; this.navigatorEnabled && a[0] && (!1 !== this.navigatorOptions.adaptToUpdatedData &&
                                                a.forEach(function (a) { q(a, "updatedData", this.updatedDataHandler) }, this), a[0].xAxis && q(a[0].xAxis, "foundExtremes", this.modifyBaseAxisExtremes))
                                        }; l.prototype.init = function (a) {
                                            var d = a.options, h = d.navigator, f = h.enabled, g = d.scrollbar, k = g.enabled; d = f ? h.height : 0; var l = k ? g.height : 0; this.handles = []; this.shades = []; this.chart = a; this.setBaseSeries(); this.height = d; this.scrollbarHeight = l; this.scrollbarEnabled = k; this.navigatorEnabled = f; this.navigatorOptions = h; this.scrollbarOptions = g; this.outlineHeight = d + l; this.opposite =
                                                C(h.opposite, !(f || !a.inverted)); var m = this; f = m.baseSeries; g = a.xAxis.length; k = a.yAxis.length; var p = f && f[0] && f[0].xAxis || a.xAxis[0] || { options: {} }; a.isDirtyBox = !0; m.navigatorEnabled ? (m.xAxis = new c(a, e({ breaks: p.options.breaks, ordinal: p.options.ordinal }, h.xAxis, { id: "navigator-x-axis", yAxis: "navigator-y-axis", isX: !0, type: "datetime", index: g, isInternal: !0, offset: 0, keepOrdinalPadding: !0, startOnTick: !1, endOnTick: !1, minPadding: 0, maxPadding: 0, zoomEnabled: !1 }, a.inverted ? { offsets: [l, 0, -l, 0], width: d } : {
                                                    offsets: [0,
                                                        -l, 0, l], height: d
                                                })), m.yAxis = new c(a, e(h.yAxis, { id: "navigator-y-axis", alignTicks: !1, offset: 0, index: k, isInternal: !0, zoomEnabled: !1 }, a.inverted ? { width: d } : { height: d })), f || h.series.data ? m.updateNavigatorSeries(!1) : 0 === a.series.length && (m.unbindRedraw = b(a, "beforeRedraw", function () { 0 < a.series.length && !m.series && (m.setBaseSeries(), m.unbindRedraw()) })), m.reversedExtremes = a.inverted && !m.xAxis.reversed || !a.inverted && m.xAxis.reversed, m.renderElements(), m.addMouseEvents()) : (m.xAxis = {
                                                    chart: a, navigatorAxis: { fake: !0 },
                                                    translate: function (b, c) { var e = a.xAxis[0], d = e.getExtremes(), h = e.len - 2 * l, f = G("min", e.options.min, d.dataMin); e = G("max", e.options.max, d.dataMax) - f; return c ? b * e / h + f : h * (b - f) / e }, toPixels: function (a) { return this.translate(a) }, toValue: function (a) { return this.translate(a, !0) }
                                                }, m.xAxis.navigatorAxis.axis = m.xAxis, m.xAxis.navigatorAxis.toFixedRange = x.AdditionsClass.prototype.toFixedRange.bind(m.xAxis.navigatorAxis)); a.options.scrollbar.enabled && (a.scrollbar = m.scrollbar = new D(a.renderer, e(a.options.scrollbar, {
                                                    margin: m.navigatorEnabled ?
                                                        0 : 10, vertical: a.inverted
                                                }), a), b(m.scrollbar, "changed", function (b) { var c = m.size, e = c * this.to; c *= this.from; m.hasDragged = m.scrollbar.hasDragged; m.render(0, 0, c, e); (a.options.scrollbar.liveRedraw || "mousemove" !== b.DOMType && "touchmove" !== b.DOMType) && setTimeout(function () { m.onMouseUp(b) }) })); m.addBaseSeriesEvents(); m.addChartEvents()
                                        }; l.prototype.getUnionExtremes = function (a) {
                                            var b = this.chart.xAxis[0], c = this.xAxis, e = c.options, d = b.options, h; a && null === b.dataMin || (h = {
                                                dataMin: C(e && e.min, G("min", d.min, b.dataMin,
                                                    c.dataMin, c.min)), dataMax: C(e && e.max, G("max", d.max, b.dataMax, c.dataMax, c.max))
                                            }); return h
                                        }; l.prototype.setBaseSeries = function (a, b) {
                                            var c = this.chart, e = this.baseSeries = []; a = a || c.options && c.options.navigator.baseSeries || (c.series.length ? p(c.series, function (a) { return !a.options.isInternal }).index : 0); (c.series || []).forEach(function (b, c) { b.options.isInternal || !b.options.showInNavigator && (c !== a && b.options.id !== a || !1 === b.options.showInNavigator) || e.push(b) }); this.xAxis && !this.xAxis.navigatorAxis.fake && this.updateNavigatorSeries(!0,
                                                b)
                                        }; l.prototype.updateNavigatorSeries = function (b, c) {
                                            var d = this, h = d.chart, g = d.baseSeries, n, l, m = d.navigatorOptions.series, p, r = { enableMouseTracking: !1, index: null, linkedTo: null, group: "nav", padXAxis: !1, xAxis: "navigator-x-axis", yAxis: "navigator-y-axis", showInLegend: !1, stacking: void 0, isInternal: !0, states: { inactive: { opacity: 1 } } }, t = d.series = (d.series || []).filter(function (a) {
                                                var b = a.baseSeries; return 0 > g.indexOf(b) ? (b && (q(b, "updatedData", d.updatedDataHandler), delete b.navigatorSeries), a.chart && a.destroy(),
                                                    !1) : !0
                                            }); g && g.length && g.forEach(function (b) {
                                                var f = b.navigatorSeries, q = E({ color: b.color, visible: b.visible }, a(m) ? k.navigator.series : m); f && !1 === d.navigatorOptions.adaptToUpdatedData || (r.name = "Navigator " + g.length, n = b.options || {}, p = n.navigatorOptions || {}, l = e(n, r, q, p), l.pointRange = C(q.pointRange, p.pointRange, k.plotOptions[l.type || "line"].pointRange), q = p.data || q.data, d.hasNavigatorData = d.hasNavigatorData || !!q, l.data = q || n.data && n.data.slice(0), f && f.options ? f.update(l, c) : (b.navigatorSeries = h.initSeries(l),
                                                    b.navigatorSeries.baseSeries = b, t.push(b.navigatorSeries)))
                                            }); if (m.data && (!g || !g.length) || a(m)) d.hasNavigatorData = !1, m = f(m), m.forEach(function (a, b) { r.name = "Navigator " + (t.length + 1); l = e(k.navigator.series, { color: h.series[b] && !h.series[b].options.isInternal && h.series[b].color || h.options.colors[b] || h.options.colors[0] }, r, a); l.data = a.data; l.data && (d.hasNavigatorData = !0, t.push(h.initSeries(l))) }); b && this.addBaseSeriesEvents()
                                        }; l.prototype.addBaseSeriesEvents = function () {
                                            var a = this, c = a.baseSeries || []; c[0] &&
                                                c[0].xAxis && b(c[0].xAxis, "foundExtremes", this.modifyBaseAxisExtremes); c.forEach(function (c) {
                                                    b(c, "show", function () { this.navigatorSeries && this.navigatorSeries.setVisible(!0, !1) }); b(c, "hide", function () { this.navigatorSeries && this.navigatorSeries.setVisible(!1, !1) }); !1 !== this.navigatorOptions.adaptToUpdatedData && c.xAxis && b(c, "updatedData", this.updatedDataHandler); b(c, "remove", function () {
                                                        this.navigatorSeries && (I(a.series, this.navigatorSeries), t(this.navigatorSeries.options) && this.navigatorSeries.remove(!1),
                                                            delete this.navigatorSeries)
                                                    })
                                                }, this)
                                        }; l.prototype.getBaseSeriesMin = function (a) { return this.baseSeries.reduce(function (a, b) { return Math.min(a, b.xData ? b.xData[0] : a) }, a) }; l.prototype.modifyNavigatorAxisExtremes = function () { var a = this.xAxis, b; "undefined" !== typeof a.getExtremes && (!(b = this.getUnionExtremes(!0)) || b.dataMin === a.min && b.dataMax === a.max || (a.min = b.dataMin, a.max = b.dataMax)) }; l.prototype.modifyBaseAxisExtremes = function () {
                                            var a = this.chart.navigator, b = this.getExtremes(), c = b.dataMin, e = b.dataMax; b =
                                                b.max - b.min; var d = a.stickToMin, f = a.stickToMax, g = C(this.options.overscroll, 0), k = a.series && a.series[0], l = !!this.setExtremes; if (!this.eventArgs || "rangeSelectorButton" !== this.eventArgs.trigger) { if (d) { var m = c; var p = m + b } f && (p = e + g, d || (m = Math.max(c, p - b, a.getBaseSeriesMin(k && k.xData ? k.xData[0] : -Number.MAX_VALUE)))); l && (d || f) && w(m) && (this.min = this.userMin = m, this.max = this.userMax = p) } a.stickToMin = a.stickToMax = null
                                        }; l.prototype.updatedDataHandler = function () {
                                            var a = this.chart.navigator, b = this.navigatorSeries, c =
                                                a.getBaseSeriesMin(this.xData[0]); a.stickToMax = a.reversedExtremes ? 0 === Math.round(a.zoomedMin) : Math.round(a.zoomedMax) >= Math.round(a.size); a.stickToMin = w(this.xAxis.min) && this.xAxis.min <= c && (!this.chart.fixedRange || !a.stickToMax); b && !a.hasNavigatorData && (b.options.pointStart = this.xData[0], b.setData(this.options.data, !1, null, !1))
                                        }; l.prototype.addChartEvents = function () {
                                            this.eventsToUnbind || (this.eventsToUnbind = []); this.eventsToUnbind.push(b(this.chart, "redraw", function () {
                                                var a = this.navigator, b = a && (a.baseSeries &&
                                                    a.baseSeries[0] && a.baseSeries[0].xAxis || this.xAxis[0]); b && a.render(b.min, b.max)
                                            }), b(this.chart, "getMargins", function () { var a = this.navigator, b = a.opposite ? "plotTop" : "marginBottom"; this.inverted && (b = a.opposite ? "marginRight" : "plotLeft"); this[b] = (this[b] || 0) + (a.navigatorEnabled || !this.inverted ? a.outlineHeight : 0) + a.navigatorOptions.margin }))
                                        }; l.prototype.destroy = function () {
                                            this.removeEvents(); this.xAxis && (I(this.chart.xAxis, this.xAxis), I(this.chart.axes, this.xAxis)); this.yAxis && (I(this.chart.yAxis, this.yAxis),
                                                I(this.chart.axes, this.yAxis)); (this.series || []).forEach(function (a) { a.destroy && a.destroy() }); "series xAxis yAxis shades outline scrollbarTrack scrollbarRifles scrollbarGroup scrollbar navigatorGroup rendered".split(" ").forEach(function (a) { this[a] && this[a].destroy && this[a].destroy(); this[a] = null }, this);[this.handles].forEach(function (a) { H(a) }, this)
                                        }; return l
                                }(); z.Navigator || (z.Navigator = B, x.compose(c), b(l, "beforeShowResetZoom", function () {
                                    var a = this.options, b = a.navigator, c = a.rangeSelector; if ((b &&
                                        b.enabled || c && c.enabled) && (!v && "x" === a.chart.zoomType || v && "x" === a.chart.pinchType)) return !1
                                }), b(l, "beforeRender", function () { var a = this.options; if (a.navigator.enabled || a.scrollbar.enabled) this.scroller = this.navigator = new B(this) }), b(l, "afterSetChartSize", function () {
                                    var a = this.legend, b = this.navigator; if (b) {
                                        var c = a && a.options; var d = b.xAxis; var e = b.yAxis; var f = b.scrollbarHeight; this.inverted ? (b.left = b.opposite ? this.chartWidth - f - b.height : this.spacing[3] + f, b.top = this.plotTop + f) : (b.left = this.plotLeft + f,
                                            b.top = b.navigatorOptions.top || this.chartHeight - b.height - f - this.spacing[2] - (this.rangeSelector && this.extraBottomMargin ? this.rangeSelector.getHeight() : 0) - (c && "bottom" === c.verticalAlign && "proximate" !== c.layout && c.enabled && !c.floating ? a.legendHeight + C(c.margin, 10) : 0) - (this.titleOffset ? this.titleOffset[2] : 0)); d && e && (this.inverted ? d.options.left = e.options.left = b.left : d.options.top = e.options.top = b.top, d.setAxisSize(), e.setAxisSize())
                                    }
                                }), b(l, "update", function (a) {
                                    var b = a.options.navigator || {}, c = a.options.scrollbar ||
                                        {}; this.navigator || this.scroller || !b.enabled && !c.enabled || (e(!0, this.options.navigator, b), e(!0, this.options.scrollbar, c), delete a.options.navigator, delete a.options.scrollbar)
                                }), b(l, "afterUpdate", function (a) { this.navigator || this.scroller || !this.options.navigator.enabled && !this.options.scrollbar.enabled || (this.scroller = this.navigator = new B(this), C(a.redraw, !0) && this.redraw(a.animation)) }), b(l, "afterAddSeries", function () { this.navigator && this.navigator.setBaseSeries(null, !1) }), b(A, "afterUpdate", function () {
                                    this.chart.navigator &&
                                    !this.options.isInternal && this.chart.navigator.setBaseSeries(null, !1)
                                }), l.prototype.callbacks.push(function (a) { var b = a.navigator; b && a.xAxis[0] && (a = a.xAxis[0].getExtremes(), b.render(a.min, a.max)) })); z.Navigator = B; return z.Navigator
                        }); J(c, "masters/modules/gantt.src.js", [], function () { })
});
//# sourceMappingURL=gantt.js.map