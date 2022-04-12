/*
 Highcharts JS v8.1.2 (2020-06-16)

 Annotations module

 (c) 2009-2019 Torstein Honsi

 License: www.highcharts.com/license
*/
(function (a) { "object" === typeof module && module.exports ? (a["default"] = a, module.exports = a) : "function" === typeof define && define.amd ? define("highcharts/modules/annotations", ["highcharts"], function (p) { a(p); a.Highcharts = p; return a }) : a("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (a) {
    function p(a, d, m, e) { a.hasOwnProperty(d) || (a[d] = e.apply(null, m)) } a = a ? a._modules : {}; p(a, "annotations/eventEmitterMixin.js", [a["parts/Globals.js"], a["parts/Utilities.js"]], function (a, d) {
        var q = d.addEvent, e = d.fireEvent,
        v = d.inArray, b = d.objectEach, A = d.pick, z = d.removeEvent; return {
            addEvents: function () {
                var c = this, h = function (h) { q(h, a.isTouchDevice ? "touchstart" : "mousedown", function (h) { c.onMouseDown(h) }) }; h(this.graphic.element); (c.labels || []).forEach(function (c) { c.options.useHTML && c.graphic.text && h(c.graphic.text.element) }); b(c.options.events, function (h, b) { var k = function (g) { "click" === b && c.cancelClick || h.call(c, c.chart.pointer.normalize(g), c.target) }; if (-1 === v(b, c.nonDOMEvents || [])) c.graphic.on(b, k); else q(c, b, k) }); if (c.options.draggable &&
                    (q(c, a.isTouchDevice ? "touchmove" : "drag", c.onDrag), !c.graphic.renderer.styledMode)) { var r = { cursor: { x: "ew-resize", y: "ns-resize", xy: "move" }[c.options.draggable] }; c.graphic.css(r); (c.labels || []).forEach(function (c) { c.options.useHTML && c.graphic.text && c.graphic.text.css(r) }) } c.isUpdating || e(c, "add")
            }, removeDocEvents: function () { this.removeDrag && (this.removeDrag = this.removeDrag()); this.removeMouseUp && (this.removeMouseUp = this.removeMouseUp()) }, onMouseDown: function (c) {
                var h = this, b = h.chart.pointer; c.preventDefault &&
                    c.preventDefault(); if (2 !== c.button) { c = b.normalize(c); var x = c.chartX; var k = c.chartY; h.cancelClick = !1; h.chart.hasDraggedAnnotation = !0; h.removeDrag = q(a.doc, a.isTouchDevice ? "touchmove" : "mousemove", function (c) { h.hasDragged = !0; c = b.normalize(c); c.prevChartX = x; c.prevChartY = k; e(h, "drag", c); x = c.chartX; k = c.chartY }); h.removeMouseUp = q(a.doc, a.isTouchDevice ? "touchend" : "mouseup", function (c) { h.cancelClick = h.hasDragged; h.hasDragged = !1; h.chart.hasDraggedAnnotation = !1; e(A(h.target, h), "afterUpdate"); h.onMouseUp(c) }) }
            },
            onMouseUp: function (c) { var h = this.chart; c = this.target || this; var b = h.options.annotations; h = h.annotations.indexOf(c); this.removeDocEvents(); b[h] = c.options }, onDrag: function (c) {
                if (this.chart.isInsidePlot(c.chartX - this.chart.plotLeft, c.chartY - this.chart.plotTop)) {
                    var b = this.mouseMoveToTranslation(c); "x" === this.options.draggable && (b.y = 0); "y" === this.options.draggable && (b.x = 0); this.points.length ? this.translate(b.x, b.y) : (this.shapes.forEach(function (c) { c.translate(b.x, b.y) }), this.labels.forEach(function (c) {
                        c.translate(b.x,
                            b.y)
                    })); this.redraw(!1)
                }
            }, mouseMoveToRadians: function (c, b, a) { var h = c.prevChartY - a, k = c.prevChartX - b; a = c.chartY - a; c = c.chartX - b; this.chart.inverted && (b = k, k = h, h = b, b = c, c = a, a = b); return Math.atan2(a, c) - Math.atan2(h, k) }, mouseMoveToTranslation: function (c) { var b = c.chartX - c.prevChartX; c = c.chartY - c.prevChartY; if (this.chart.inverted) { var a = c; c = b; b = a } return { x: b, y: c } }, mouseMoveToScale: function (c, b, a) {
                b = (c.chartX - b || 1) / (c.prevChartX - b || 1); c = (c.chartY - a || 1) / (c.prevChartY - a || 1); this.chart.inverted && (a = c, c = b, b = a); return {
                    x: b,
                    y: c
                }
            }, destroy: function () { this.removeDocEvents(); z(this); this.hcEvents = null }
        }
    }); p(a, "annotations/ControlPoint.js", [a["parts/Utilities.js"], a["annotations/eventEmitterMixin.js"]], function (a, d) {
        var q = a.merge, e = a.pick; return function () {
            function a(b, a, v, c) {
                this.addEvents = d.addEvents; this.graphic = void 0; this.mouseMoveToRadians = d.mouseMoveToRadians; this.mouseMoveToScale = d.mouseMoveToScale; this.mouseMoveToTranslation = d.mouseMoveToTranslation; this.onDrag = d.onDrag; this.onMouseDown = d.onMouseDown; this.onMouseUp =
                    d.onMouseUp; this.removeDocEvents = d.removeDocEvents; this.nonDOMEvents = ["drag"]; this.chart = b; this.target = a; this.options = v; this.index = e(v.index, c)
            } a.prototype.setVisibility = function (b) { this.graphic.attr("visibility", b ? "visible" : "hidden"); this.options.visible = b }; a.prototype.render = function () { var b = this.chart, a = this.options; this.graphic = b.renderer.symbol(a.symbol, 0, 0, a.width, a.height).add(b.controlPointsGroup).css(a.style); this.setVisibility(a.visible); this.addEvents() }; a.prototype.redraw = function (b) {
                this.graphic[b ?
                    "animate" : "attr"](this.options.positioner.call(this, this.target))
            }; a.prototype.destroy = function () { d.destroy.call(this); this.graphic && (this.graphic = this.graphic.destroy()); this.options = this.target = this.chart = null }; a.prototype.update = function (b) { var a = this.chart, d = this.target, c = this.index; b = q(!0, this.options, b); this.destroy(); this.constructor(a, d, b, c); this.render(a.controlPointsGroup); this.redraw() }; return a
        }()
    }); p(a, "annotations/MockPoint.js", [a["parts/Globals.js"], a["parts/Utilities.js"]], function (a,
        d) {
            var q = d.defined, e = d.fireEvent; return function () {
                function d(b, d, e) { this.y = this.x = this.plotY = this.plotX = this.isInside = void 0; this.mock = !0; this.series = { visible: !0, chart: b, getPlotBox: a.Series.prototype.getPlotBox }; this.target = d || null; this.options = e; this.applyOptions(this.getOptions()) } d.fromPoint = function (b) { return new d(b.series.chart, null, { x: b.x, y: b.y, xAxis: b.series.xAxis, yAxis: b.series.yAxis }) }; d.pointToPixels = function (b, a) {
                    var d = b.series, c = d.chart, h = b.plotX, r = b.plotY; c.inverted && (b.mock ? (h = b.plotY,
                        r = b.plotX) : (h = c.plotWidth - b.plotY, r = c.plotHeight - b.plotX)); d && !a && (b = d.getPlotBox(), h += b.translateX, r += b.translateY); return { x: h, y: r }
                }; d.pointToOptions = function (b) { return { x: b.x, y: b.y, xAxis: b.series.xAxis, yAxis: b.series.yAxis } }; d.prototype.hasDynamicOptions = function () { return "function" === typeof this.options }; d.prototype.getOptions = function () { return this.hasDynamicOptions() ? this.options(this.target) : this.options }; d.prototype.applyOptions = function (b) {
                    this.command = b.command; this.setAxis(b, "x"); this.setAxis(b,
                        "y"); this.refresh()
                }; d.prototype.setAxis = function (b, d) { d += "Axis"; b = b[d]; var e = this.series.chart; this.series[d] = b instanceof a.Axis ? b : q(b) ? e[d][b] || e.get(b) : null }; d.prototype.toAnchor = function () { var b = [this.plotX, this.plotY, 0, 0]; this.series.chart.inverted && (b[0] = this.plotY, b[1] = this.plotX); return b }; d.prototype.getLabelConfig = function () { return { x: this.x, y: this.y, point: this } }; d.prototype.isInsidePlot = function () {
                    var b = this.plotX, a = this.plotY, d = this.series.xAxis, c = this.series.yAxis, h = { x: b, y: a, isInsidePlot: !0 };
                    d && (h.isInsidePlot = q(b) && 0 <= b && b <= d.len); c && (h.isInsidePlot = h.isInsidePlot && q(a) && 0 <= a && a <= c.len); e(this.series.chart, "afterIsInsidePlot", h); return h.isInsidePlot
                }; d.prototype.refresh = function () { var b = this.series, a = b.xAxis; b = b.yAxis; var d = this.getOptions(); a ? (this.x = d.x, this.plotX = a.toPixels(d.x, !0)) : (this.x = null, this.plotX = d.x); b ? (this.y = d.y, this.plotY = b.toPixels(d.y, !0)) : (this.y = null, this.plotY = d.y); this.isInside = this.isInsidePlot() }; d.prototype.translate = function (b, a, d, c) {
                    this.hasDynamicOptions() ||
                    (this.plotX += d, this.plotY += c, this.refreshOptions())
                }; d.prototype.scale = function (b, a, d, c) { if (!this.hasDynamicOptions()) { var h = this.plotY * c; this.plotX = (1 - d) * b + this.plotX * d; this.plotY = (1 - c) * a + h; this.refreshOptions() } }; d.prototype.rotate = function (b, a, d) { if (!this.hasDynamicOptions()) { var c = Math.cos(d); d = Math.sin(d); var h = this.plotX, e = this.plotY; h -= b; e -= a; this.plotX = h * c - e * d + b; this.plotY = h * d + e * c + a; this.refreshOptions() } }; d.prototype.refreshOptions = function () {
                    var b = this.series, a = b.xAxis; b = b.yAxis; this.x =
                        this.options.x = a ? this.options.x = a.toValue(this.plotX, !0) : this.plotX; this.y = this.options.y = b ? b.toValue(this.plotY, !0) : this.plotY
                }; return d
            }()
    }); p(a, "annotations/controllable/controllableMixin.js", [a["annotations/ControlPoint.js"], a["annotations/MockPoint.js"], a["parts/Tooltip.js"], a["parts/Utilities.js"]], function (a, d, m, e) {
        var q = e.isObject, b = e.isString, A = e.merge, z = e.splat; return {
            init: function (c, b, a) {
                this.annotation = c; this.chart = c.chart; this.options = b; this.points = []; this.controlPoints = []; this.index =
                    a; this.linkPoints(); this.addControlPoints()
            }, attr: function () { this.graphic.attr.apply(this.graphic, arguments) }, getPointsOptions: function () { var c = this.options; return c.points || c.point && z(c.point) }, attrsFromOptions: function (c) { var b = this.constructor.attrsMap, a = {}, d, k = this.chart.styledMode; for (d in c) { var e = b[d]; !e || k && -1 !== ["fill", "stroke", "stroke-width"].indexOf(e) || (a[e] = c[d]) } return a }, anchor: function (c) {
                var b = c.series.getPlotBox(); c = c.mock ? c.toAnchor() : m.prototype.getAnchor.call({ chart: c.series.chart },
                    c); c = { x: c[0] + (this.options.x || 0), y: c[1] + (this.options.y || 0), height: c[2] || 0, width: c[3] || 0 }; return { relativePosition: c, absolutePosition: A(c, { x: c.x + b.translateX, y: c.y + b.translateY }) }
            }, point: function (c, a) { if (c && c.series) return c; a && null !== a.series || (q(c) ? a = new d(this.chart, this, c) : b(c) ? a = this.chart.get(c) || null : "function" === typeof c && (a = c.call(a, this), a = a.series ? a : new d(this.chart, this, c))); return a }, linkPoints: function () {
                var c = this.getPointsOptions(), b = this.points, a = c && c.length || 0, d; for (d = 0; d < a; d++) {
                    var k =
                        this.point(c[d], b[d]); if (!k) { b.length = 0; return } k.mock && k.refresh(); b[d] = k
                } return b
            }, addControlPoints: function () { var c = this.options.controlPoints; (c || []).forEach(function (b, d) { b = A(this.options.controlPointOptions, b); b.index || (b.index = d); c[d] = b; this.controlPoints.push(new a(this.chart, this, b)) }, this) }, shouldBeDrawn: function () { return !!this.points.length }, render: function (c) { this.controlPoints.forEach(function (c) { c.render() }) }, redraw: function (c) { this.controlPoints.forEach(function (b) { b.redraw(c) }) }, transform: function (c,
                b, a, d, k) { if (this.chart.inverted) { var h = b; b = a; a = h } this.points.forEach(function (g, f) { this.transformPoint(c, b, a, d, k, f) }, this) }, transformPoint: function (c, b, a, e, k, y) { var g = this.points[y]; g.mock || (g = this.points[y] = d.fromPoint(g)); g[c](b, a, e, k) }, translate: function (c, b) { this.transform("translate", null, null, c, b) }, translatePoint: function (c, b, a) { this.transformPoint("translate", null, null, c, b, a) }, translateShape: function (c, b) {
                    var a = this.annotation.chart, d = this.annotation.userOptions, k = a.annotations.indexOf(this.annotation);
                    a = a.options.annotations[k]; this.translatePoint(c, b, 0); a[this.collection][this.index].point = this.options.point; d[this.collection][this.index].point = this.options.point
                }, rotate: function (c, b, a) { this.transform("rotate", c, b, a) }, scale: function (c, b, a, d) { this.transform("scale", c, b, a, d) }, setControlPointsVisibility: function (b) { this.controlPoints.forEach(function (c) { c.setVisibility(b) }) }, destroy: function () {
                    this.graphic && (this.graphic = this.graphic.destroy()); this.tracker && (this.tracker = this.tracker.destroy());
                    this.controlPoints.forEach(function (b) { b.destroy() }); this.options = this.controlPoints = this.points = this.chart = null; this.annotation && (this.annotation = null)
                }, update: function (b) { var c = this.annotation; b = A(!0, this.options, b); var a = this.graphic.parentGroup; this.destroy(); this.constructor(c, b); this.render(a); this.redraw() }
        }
    }); p(a, "annotations/controllable/markerMixin.js", [a["parts/Globals.js"], a["parts/Utilities.js"]], function (a, d) {
        var q = d.addEvent, e = d.defined, v = d.merge, b = d.objectEach, A = d.uniqueKey, z = {
            arrow: {
                tagName: "marker",
                render: !1, id: "arrow", refY: 5, refX: 9, markerWidth: 10, markerHeight: 10, children: [{ tagName: "path", d: "M 0 0 L 10 5 L 0 10 Z", strokeWidth: 0 }]
            }, "reverse-arrow": { tagName: "marker", render: !1, id: "reverse-arrow", refY: 5, refX: 1, markerWidth: 10, markerHeight: 10, children: [{ tagName: "path", d: "M 0 5 L 10 0 L 10 10 Z", strokeWidth: 0 }] }
        }; a.SVGRenderer.prototype.addMarker = function (b, a) {
            var c = { id: b }, d = { stroke: a.color || "none", fill: a.color || "rgba(0, 0, 0, 0.75)" }; c.children = a.children.map(function (b) { return v(d, b) }); a = this.definition(v(!0,
                { markerWidth: 20, markerHeight: 20, refX: 0, refY: 0, orient: "auto" }, a, c)); a.id = b; return a
        }; d = function (b) { return function (c) { this.attr(b, "url(#" + c + ")") } }; d = {
            markerEndSetter: d("marker-end"), markerStartSetter: d("marker-start"), setItemMarkers: function (b) {
                var c = b.options, a = b.chart, d = a.options.defs, k = c.fill, y = e(k) && "none" !== k ? k : c.stroke;["markerStart", "markerEnd"].forEach(function (g) {
                    var f = c[g], k; if (f) {
                        for (k in d) { var e = d[k]; if (f === e.id && "marker" === e.tagName) { var l = e; break } } l && (f = b[g] = a.renderer.addMarker((c.id ||
                            A()) + "-" + l.id, v(l, { color: y })), b.attr(g, f.attr("id")))
                    }
                })
            }
        }; q(a.Chart, "afterGetContainer", function () { this.options.defs = v(z, this.options.defs || {}); b(this.options.defs, function (b) { "marker" === b.tagName && !1 !== b.render && this.renderer.addMarker(b.id, b) }, this) }); return d
    }); p(a, "annotations/controllable/ControllablePath.js", [a["annotations/controllable/controllableMixin.js"], a["parts/Globals.js"], a["annotations/controllable/markerMixin.js"], a["parts/Utilities.js"]], function (a, d, m, e) {
        var q = e.extend; e = e.merge;
        var b = "rgba(192,192,192," + (d.svg ? .0001 : .002) + ")"; d = function (b, a, c) { this.init(b, a, c); this.collection = "shapes" }; d.attrsMap = { dashStyle: "dashstyle", strokeWidth: "stroke-width", stroke: "stroke", fill: "fill", zIndex: "zIndex" }; e(!0, d.prototype, a, {
            type: "path", setMarkers: m.setItemMarkers, toD: function () {
                var b = this.options.d; if (b) return "function" === typeof b ? b.call(this) : b; b = this.points; var a = b.length, c = a, d = b[0], e = c && this.anchor(d).absolutePosition, x = 0, k = []; if (e) for (k.push(["M", e.x, e.y]); ++x < a && c;)d = b[x], c = d.command ||
                    "L", e = this.anchor(d).absolutePosition, "M" === c ? k.push([c, e.x, e.y]) : "L" === c ? k.push([c, e.x, e.y]) : "Z" === c && k.push([c]), c = d.series.visible; return c ? this.chart.renderer.crispLine(k, this.graphic.strokeWidth()) : null
            }, shouldBeDrawn: function () { return a.shouldBeDrawn.call(this) || !!this.options.d }, render: function (d) {
                var e = this.options, c = this.attrsFromOptions(e); this.graphic = this.annotation.chart.renderer.path([["M", 0, 0]]).attr(c).add(d); e.className && this.graphic.addClass(e.className); this.tracker = this.annotation.chart.renderer.path([["M",
                    0, 0]]).addClass("highcharts-tracker-line").attr({ zIndex: 2 }).add(d); this.annotation.chart.styledMode || this.tracker.attr({ "stroke-linejoin": "round", stroke: b, fill: b, "stroke-width": this.graphic.strokeWidth() + 2 * e.snap }); a.render.call(this); q(this.graphic, { markerStartSetter: m.markerStartSetter, markerEndSetter: m.markerEndSetter }); this.setMarkers(this)
            }, redraw: function (b) {
                var d = this.toD(), c = b ? "animate" : "attr"; d ? (this.graphic[c]({ d: d }), this.tracker[c]({ d: d })) : (this.graphic.attr({ d: "M 0 -9000000000" }), this.tracker.attr({ d: "M 0 -9000000000" }));
                this.graphic.placed = this.tracker.placed = !!d; a.redraw.call(this, b)
            }
        }); return d
    }); p(a, "annotations/controllable/ControllableRect.js", [a["annotations/controllable/controllableMixin.js"], a["annotations/controllable/ControllablePath.js"], a["parts/Utilities.js"]], function (a, d, m) {
        m = m.merge; var e = function (a, b, d) { this.init(a, b, d); this.collection = "shapes" }; e.attrsMap = m(d.attrsMap, { width: "width", height: "height" }); m(!0, e.prototype, a, {
            type: "rect", translate: a.translateShape, render: function (d) {
                var b = this.attrsFromOptions(this.options);
                this.graphic = this.annotation.chart.renderer.rect(0, -9E9, 0, 0).attr(b).add(d); a.render.call(this)
            }, redraw: function (d) { var b = this.anchor(this.points[0]).absolutePosition; if (b) this.graphic[d ? "animate" : "attr"]({ x: b.x, y: b.y, width: this.options.width, height: this.options.height }); else this.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!b; a.redraw.call(this, d) }
        }); return e
    }); p(a, "annotations/controllable/ControllableCircle.js", [a["annotations/controllable/controllableMixin.js"], a["annotations/controllable/ControllablePath.js"],
    a["parts/Utilities.js"]], function (a, d, m) {
        m = m.merge; var e = function (a, b, d) { this.init(a, b, d); this.collection = "shapes" }; e.attrsMap = m(d.attrsMap, { r: "r" }); m(!0, e.prototype, a, {
            type: "circle", translate: a.translateShape, render: function (d) { var b = this.attrsFromOptions(this.options); this.graphic = this.annotation.chart.renderer.circle(0, -9E9, 0).attr(b).add(d); a.render.call(this) }, redraw: function (d) {
                var b = this.anchor(this.points[0]).absolutePosition; if (b) this.graphic[d ? "animate" : "attr"]({ x: b.x, y: b.y, r: this.options.r });
                else this.graphic.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!b; a.redraw.call(this, d)
            }, setRadius: function (a) { this.options.r = a }
        }); return e
    }); p(a, "annotations/controllable/ControllableLabel.js", [a["annotations/controllable/controllableMixin.js"], a["parts/Globals.js"], a["annotations/MockPoint.js"], a["parts/Tooltip.js"], a["parts/Utilities.js"]], function (a, d, m, e, v) {
        var b = v.extend, q = v.format, p = v.isNumber, c = v.merge, h = v.pick, r = function (b, a, c) { this.init(b, a, c); this.collection = "labels" }; r.shapesWithoutBackground =
            ["connector"]; r.alignedPosition = function (b, a) { var c = b.align, d = b.verticalAlign, f = (a.x || 0) + (b.x || 0), k = (a.y || 0) + (b.y || 0), e, l; "right" === c ? e = 1 : "center" === c && (e = 2); e && (f += (a.width - (b.width || 0)) / e); "bottom" === d ? l = 1 : "middle" === d && (l = 2); l && (k += (a.height - (b.height || 0)) / l); return { x: Math.round(f), y: Math.round(k) } }; r.justifiedOptions = function (b, a, c, d) {
                var f = c.align, g = c.verticalAlign, e = a.box ? 0 : a.padding || 0, k = a.getBBox(); a = { align: f, verticalAlign: g, x: c.x, y: c.y, width: a.width, height: a.height }; c = d.x - b.plotLeft; var t =
                    d.y - b.plotTop; d = c + e; 0 > d && ("right" === f ? a.align = "left" : a.x = -d); d = c + k.width - e; d > b.plotWidth && ("left" === f ? a.align = "right" : a.x = b.plotWidth - d); d = t + e; 0 > d && ("bottom" === g ? a.verticalAlign = "top" : a.y = -d); d = t + k.height - e; d > b.plotHeight && ("top" === g ? a.verticalAlign = "bottom" : a.y = b.plotHeight - d); return a
            }; r.attrsMap = { backgroundColor: "fill", borderColor: "stroke", borderWidth: "stroke-width", zIndex: "zIndex", borderRadius: "r", padding: "padding" }; c(!0, r.prototype, a, {
                translatePoint: function (b, c) {
                    a.translatePoint.call(this, b,
                        c, 0)
                }, translate: function (b, a) { var c = this.annotation.chart, d = this.annotation.userOptions, f = c.annotations.indexOf(this.annotation); f = c.options.annotations[f]; c.inverted && (c = b, b = a, a = c); this.options.x += b; this.options.y += a; f[this.collection][this.index].x = this.options.x; f[this.collection][this.index].y = this.options.y; d[this.collection][this.index].x = this.options.x; d[this.collection][this.index].y = this.options.y }, render: function (b) {
                    var c = this.options, d = this.attrsFromOptions(c), g = c.style; this.graphic = this.annotation.chart.renderer.label("",
                        0, -9999, c.shape, null, null, c.useHTML, null, "annotation-label").attr(d).add(b); this.annotation.chart.styledMode || ("contrast" === g.color && (g.color = this.annotation.chart.renderer.getContrast(-1 < r.shapesWithoutBackground.indexOf(c.shape) ? "#FFFFFF" : c.backgroundColor)), this.graphic.css(c.style).shadow(c.shadow)); c.className && this.graphic.addClass(c.className); this.graphic.labelrank = c.labelrank; a.render.call(this)
                }, redraw: function (b) {
                    var c = this.options, d = this.text || c.format || c.text, g = this.graphic, f = this.points[0];
                    g.attr({ text: d ? q(d, f.getLabelConfig(), this.annotation.chart) : c.formatter.call(f, this) }); c = this.anchor(f); (d = this.position(c)) ? (g.alignAttr = d, d.anchorX = c.absolutePosition.x, d.anchorY = c.absolutePosition.y, g[b ? "animate" : "attr"](d)) : g.attr({ x: 0, y: -9999 }); g.placed = !!d; a.redraw.call(this, b)
                }, anchor: function () { var b = a.anchor.apply(this, arguments), c = this.options.x || 0, d = this.options.y || 0; b.absolutePosition.x -= c; b.absolutePosition.y -= d; b.relativePosition.x -= c; b.relativePosition.y -= d; return b }, position: function (c) {
                    var a =
                        this.graphic, d = this.annotation.chart, g = this.points[0], f = this.options, n = c.absolutePosition, u = c.relativePosition; if (c = g.series.visible && m.prototype.isInsidePlot.call(g)) {
                            if (f.distance) var l = e.prototype.getPosition.call({ chart: d, distance: h(f.distance, 16) }, a.width, a.height, { plotX: u.x, plotY: u.y, negative: g.negative, ttBelow: g.ttBelow, h: u.height || u.width }); else f.positioner ? l = f.positioner.call(this) : (g = { x: n.x, y: n.y, width: 0, height: 0 }, l = r.alignedPosition(b(f, { width: a.width, height: a.height }), g), "justify" ===
                                this.options.overflow && (l = r.alignedPosition(r.justifiedOptions(d, a, f, l), g))); f.crop && (f = l.x - d.plotLeft, g = l.y - d.plotTop, c = d.isInsidePlot(f, g) && d.isInsidePlot(f + a.width, g + a.height))
                        } return c ? l : null
                }
            }); d.SVGRenderer.prototype.symbols.connector = function (b, c, a, d, f) {
                var g = f && f.anchorX; f = f && f.anchorY; var e = a / 2; if (p(g) && p(f)) { var l = [["M", g, f]]; var t = c - f; 0 > t && (t = -d - t); t < a && (e = g < b + a / 2 ? t : a - t); f > c + d ? l.push(["L", b + e, c + d]) : f < c ? l.push(["L", b + e, c]) : g < b ? l.push(["L", b, c + d / 2]) : g > b + a && l.push(["L", b + a, c + d / 2]) } return l ||
                    []
            }; return r
    }); p(a, "annotations/controllable/ControllableImage.js", [a["annotations/controllable/ControllableLabel.js"], a["annotations/controllable/controllableMixin.js"], a["parts/Utilities.js"]], function (a, d, m) {
        m = m.merge; var e = function (a, b, d) { this.init(a, b, d); this.collection = "shapes" }; e.attrsMap = { width: "width", height: "height", zIndex: "zIndex" }; m(!0, e.prototype, d, {
            type: "image", translate: d.translateShape, render: function (a) {
                var b = this.attrsFromOptions(this.options), e = this.options; this.graphic = this.annotation.chart.renderer.image(e.src,
                    0, -9E9, e.width, e.height).attr(b).add(a); this.graphic.width = e.width; this.graphic.height = e.height; d.render.call(this)
            }, redraw: function (e) { var b = this.anchor(this.points[0]); if (b = a.prototype.position.call(this, b)) this.graphic[e ? "animate" : "attr"]({ x: b.x, y: b.y }); else this.graphic.attr({ x: 0, y: -9E9 }); this.graphic.placed = !!b; d.redraw.call(this, e) }
        }); return e
    }); p(a, "annotations/annotations.src.js", [a["parts/Chart.js"], a["annotations/controllable/controllableMixin.js"], a["annotations/controllable/ControllableRect.js"],
    a["annotations/controllable/ControllableCircle.js"], a["annotations/controllable/ControllablePath.js"], a["annotations/controllable/ControllableImage.js"], a["annotations/controllable/ControllableLabel.js"], a["annotations/ControlPoint.js"], a["annotations/eventEmitterMixin.js"], a["parts/Globals.js"], a["annotations/MockPoint.js"], a["parts/Pointer.js"], a["parts/Utilities.js"]], function (a, d, m, e, v, b, p, z, c, h, r, x, k) {
        a = a.prototype; var q = k.addEvent, g = k.defined, f = k.destroyObjectProperties, n = k.erase, u = k.extend,
            l = k.find, t = k.fireEvent, w = k.merge, C = k.pick, D = k.splat; k = k.wrap; var B = function () {
                function a(a, b) {
                    this.annotation = void 0; this.coll = "annotations"; this.shapesGroup = this.labelsGroup = this.labelCollector = this.group = this.graphic = this.collection = void 0; this.chart = a; this.points = []; this.controlPoints = []; this.coll = "annotations"; this.labels = []; this.shapes = []; this.options = w(this.defaultOptions, b); this.userOptions = b; b = this.getLabelsAndShapesOptions(this.options, b); this.options.labels = b.labels; this.options.shapes =
                        b.shapes; this.init(a, this.options)
                } a.prototype.init = function () { this.linkPoints(); this.addControlPoints(); this.addShapes(); this.addLabels(); this.setLabelCollector() }; a.prototype.getLabelsAndShapesOptions = function (a, b) { var c = {};["labels", "shapes"].forEach(function (d) { a[d] && (c[d] = D(b[d]).map(function (b, c) { return w(a[d][c], b) })) }); return c }; a.prototype.addShapes = function () { (this.options.shapes || []).forEach(function (a, b) { a = this.initShape(a, b); w(!0, this.options.shapes[b], a.options) }, this) }; a.prototype.addLabels =
                    function () { (this.options.labels || []).forEach(function (a, b) { a = this.initLabel(a, b); w(!0, this.options.labels[b], a.options) }, this) }; a.prototype.addClipPaths = function () { this.setClipAxes(); this.clipXAxis && this.clipYAxis && (this.clipRect = this.chart.renderer.clipRect(this.getClipBox())) }; a.prototype.setClipAxes = function () {
                        var a = this.chart.xAxis, b = this.chart.yAxis, c = (this.options.labels || []).concat(this.options.shapes || []).reduce(function (c, d) {
                            return [a[d && d.point && d.point.xAxis] || c[0], b[d && d.point && d.point.yAxis] ||
                                c[1]]
                        }, []); this.clipXAxis = c[0]; this.clipYAxis = c[1]
                    }; a.prototype.getClipBox = function () { if (this.clipXAxis && this.clipYAxis) return { x: this.clipXAxis.left, y: this.clipYAxis.top, width: this.clipXAxis.width, height: this.clipYAxis.height } }; a.prototype.setLabelCollector = function () { var a = this; a.labelCollector = function () { return a.labels.reduce(function (a, b) { b.options.allowOverlap || a.push(b.graphic); return a }, []) }; a.chart.labelCollectors.push(a.labelCollector) }; a.prototype.setOptions = function (a) {
                        this.options = w(this.defaultOptions,
                            a)
                    }; a.prototype.redraw = function (a) { this.linkPoints(); this.graphic || this.render(); this.clipRect && this.clipRect.animate(this.getClipBox()); this.redrawItems(this.shapes, a); this.redrawItems(this.labels, a); d.redraw.call(this, a) }; a.prototype.redrawItems = function (a, b) { for (var c = a.length; c--;)this.redrawItem(a[c], b) }; a.prototype.renderItems = function (a) { for (var b = a.length; b--;)this.renderItem(a[b]) }; a.prototype.render = function () {
                        var a = this.chart.renderer; this.graphic = a.g("annotation").attr({
                            zIndex: this.options.zIndex,
                            visibility: this.options.visible ? "visible" : "hidden"
                        }).add(); this.shapesGroup = a.g("annotation-shapes").add(this.graphic).clip(this.chart.plotBoxClip); this.labelsGroup = a.g("annotation-labels").attr({ translateX: 0, translateY: 0 }).add(this.graphic); this.addClipPaths(); this.clipRect && this.graphic.clip(this.clipRect); this.renderItems(this.shapes); this.renderItems(this.labels); this.addEvents(); d.render.call(this)
                    }; a.prototype.setVisibility = function (a) {
                        var b = this.options; a = C(a, !b.visible); this.graphic.attr("visibility",
                            a ? "visible" : "hidden"); a || this.setControlPointsVisibility(!1); b.visible = a
                    }; a.prototype.setControlPointsVisibility = function (a) { var b = function (b) { b.setControlPointsVisibility(a) }; d.setControlPointsVisibility.call(this, a); this.shapes.forEach(b); this.labels.forEach(b) }; a.prototype.destroy = function () {
                        var a = this.chart, b = function (a) { a.destroy() }; this.labels.forEach(b); this.shapes.forEach(b); this.clipYAxis = this.clipXAxis = null; n(a.labelCollectors, this.labelCollector); c.destroy.call(this); d.destroy.call(this);
                        f(this, a)
                    }; a.prototype.remove = function () { return this.chart.removeAnnotation(this) }; a.prototype.update = function (a, b) { var c = this.chart, d = this.getLabelsAndShapesOptions(this.userOptions, a), f = c.annotations.indexOf(this); a = w(!0, this.userOptions, a); a.labels = d.labels; a.shapes = d.shapes; this.destroy(); this.constructor(c, a); c.options.annotations[f] = a; this.isUpdating = !0; C(b, !0) && c.redraw(); t(this, "afterUpdate"); this.isUpdating = !1 }; a.prototype.initShape = function (b, c) {
                        b = w(this.options.shapeOptions, { controlPointOptions: this.options.controlPointOptions },
                            b); c = new a.shapesMap[b.type](this, b, c); c.itemType = "shape"; this.shapes.push(c); return c
                    }; a.prototype.initLabel = function (a, b) { a = w(this.options.labelOptions, { controlPointOptions: this.options.controlPointOptions }, a); b = new p(this, a, b); b.itemType = "label"; this.labels.push(b); return b }; a.prototype.redrawItem = function (a, b) { a.linkPoints(); a.shouldBeDrawn() ? (a.graphic || this.renderItem(a), a.redraw(C(b, !0) && a.graphic.placed), a.points.length && this.adjustVisibility(a)) : this.destroyItem(a) }; a.prototype.adjustVisibility =
                        function (a) { var b = !1, c = a.graphic; a.points.forEach(function (a) { !1 !== a.series.visible && !1 !== a.visible && (b = !0) }); b ? "hidden" === c.visibility && c.show() : c.hide() }; a.prototype.destroyItem = function (a) { n(this[a.itemType + "s"], a); a.destroy() }; a.prototype.renderItem = function (a) { a.render("label" === a.itemType ? this.labelsGroup : this.shapesGroup) }; a.ControlPoint = z; a.MockPoint = r; a.shapesMap = { rect: m, circle: e, path: v, image: b }; a.types = {}; return a
            }(); w(!0, B.prototype, d, c, w(B.prototype, {
                nonDOMEvents: ["add", "afterUpdate",
                    "drag", "remove"], defaultOptions: {
                        visible: !0, draggable: "xy", labelOptions: { align: "center", allowOverlap: !1, backgroundColor: "rgba(0, 0, 0, 0.75)", borderColor: "black", borderRadius: 3, borderWidth: 1, className: "", crop: !1, formatter: function () { return g(this.y) ? this.y : "Annotation label" }, overflow: "justify", padding: 5, shadow: !1, shape: "callout", style: { fontSize: "11px", fontWeight: "normal", color: "contrast" }, useHTML: !1, verticalAlign: "bottom", x: 0, y: -16 }, shapeOptions: {
                            stroke: "rgba(0, 0, 0, 0.75)", strokeWidth: 1, fill: "rgba(0, 0, 0, 0.75)",
                            r: 0, snap: 2
                        }, controlPointOptions: { symbol: "circle", width: 10, height: 10, style: { stroke: "black", "stroke-width": 2, fill: "white" }, visible: !1, events: {} }, events: {}, zIndex: 6
                    }
            })); h.extendAnnotation = function (a, b, c, d) { b = b || B; w(!0, a.prototype, b.prototype, c); a.prototype.defaultOptions = w(a.prototype.defaultOptions, d || {}) }; u(a, {
                initAnnotation: function (a) { a = new (B.types[a.type] || B)(this, a); this.annotations.push(a); return a }, addAnnotation: function (a, b) {
                    a = this.initAnnotation(a); this.options.annotations.push(a.options);
                    C(b, !0) && a.redraw(); return a
                }, removeAnnotation: function (a) { var b = this.annotations, c = "annotations" === a.coll ? a : l(b, function (b) { return b.options.id === a }); c && (t(c, "remove"), n(this.options.annotations, c.options), n(b, c), c.destroy()) }, drawAnnotations: function () { this.plotBoxClip.attr(this.plotBox); this.annotations.forEach(function (a) { a.redraw() }) }
            }); a.collectionsWithUpdate.push("annotations"); a.collectionsWithInit.annotations = [a.addAnnotation]; a.callbacks.push(function (a) {
                a.annotations = []; a.options.annotations ||
                    (a.options.annotations = []); a.plotBoxClip = this.renderer.clipRect(this.plotBox); a.controlPointsGroup = a.renderer.g("control-points").attr({ zIndex: 99 }).clip(a.plotBoxClip).add(); a.options.annotations.forEach(function (b, c) { b = a.initAnnotation(b); a.options.annotations[c] = b.options }); a.drawAnnotations(); q(a, "redraw", a.drawAnnotations); q(a, "destroy", function () { a.plotBoxClip.destroy(); a.controlPointsGroup.destroy() })
            }); k(x.prototype, "onContainerMouseDown", function (a) {
                this.chart.hasDraggedAnnotation || a.apply(this,
                    Array.prototype.slice.call(arguments, 1))
            }); return h.Annotation = B
    }); p(a, "mixins/navigation.js", [], function () { return { initUpdate: function (a) { a.navigation || (a.navigation = { updates: [], update: function (a, m) { this.updates.forEach(function (d) { d.update.call(d.context, a, m) }) } }) }, addUpdate: function (a, d) { d.navigation || this.initUpdate(d); d.navigation.updates.push({ update: a, context: d }) } } }); p(a, "annotations/navigationBindings.js", [a["annotations/annotations.src.js"], a["mixins/navigation.js"], a["parts/Globals.js"],
    a["parts/Utilities.js"]], function (a, d, m, e) {
        function q(a) {
            var b = a.prototype.defaultOptions.events && a.prototype.defaultOptions.events.click; y(!0, a.prototype.defaultOptions.events, {
                click: function (a) {
                    var d = this, f = d.chart.navigationBindings, g = f.activeAnnotation; b && b.call(d, a); g !== d ? (f.deselectAnnotation(), f.activeAnnotation = d, d.setControlPointsVisibility(!0), c(f, "showPopup", {
                        annotation: d, formType: "annotation-toolbar", options: f.annotationToFields(d), onSubmit: function (a) {
                            var b = {}; "remove" === a.actionType ?
                                (f.activeAnnotation = !1, f.chart.removeAnnotation(d)) : (f.fieldsToOptions(a.fields, b), f.deselectAnnotation(), a = b.typeOptions, "measure" === d.options.type && (a.crosshairY.enabled = 0 !== a.crosshairY.strokeWidth, a.crosshairX.enabled = 0 !== a.crosshairX.strokeWidth), d.update(b))
                        }
                    })) : (f.deselectAnnotation(), c(f, "closePopup")); a.activeAnnotation = !0
                }
            })
        } var b = e.addEvent, p = e.attr, z = e.format, c = e.fireEvent, h = e.isArray, r = e.isFunction, x = e.isNumber, k = e.isObject, y = e.merge, g = e.objectEach, f = e.pick; e = e.setOptions; var n = m.doc,
            u = m.win, l = function () {
                function a(a, b) { this.selectedButton = this.boundClassNames = void 0; this.chart = a; this.options = b; this.eventsToUnbind = []; this.container = n.getElementsByClassName(this.options.bindingsClassName || "") } a.prototype.initEvents = function () {
                    var a = this, c = a.chart, d = a.container, f = a.options; a.boundClassNames = {}; g(f.bindings || {}, function (b) { a.boundClassNames[b.className] = b });[].forEach.call(d, function (c) {
                        a.eventsToUnbind.push(b(c, "click", function (b) {
                            var d = a.getButtonEvents(c, b); d && a.bindingsButtonClick(d.button,
                                d.events, b)
                        }))
                    }); g(f.events || {}, function (c, d) { r(c) && a.eventsToUnbind.push(b(a, d, c)) }); a.eventsToUnbind.push(b(c.container, "click", function (b) { !c.cancelClick && c.isInsidePlot(b.chartX - c.plotLeft, b.chartY - c.plotTop) && a.bindingsChartClick(this, b) })); a.eventsToUnbind.push(b(c.container, m.isTouchDevice ? "touchmove" : "mousemove", function (b) { a.bindingsContainerMouseMove(this, b) }))
                }; a.prototype.initUpdate = function () { var a = this; d.addUpdate(function (b) { a.update(b) }, this.chart) }; a.prototype.bindingsButtonClick =
                    function (a, b, d) { var f = this.chart; this.selectedButtonElement && (c(this, "deselectButton", { button: this.selectedButtonElement }), this.nextEvent && (this.currentUserDetails && "annotations" === this.currentUserDetails.coll && f.removeAnnotation(this.currentUserDetails), this.mouseMoveEvent = this.nextEvent = !1)); this.selectedButton = b; this.selectedButtonElement = a; c(this, "selectButton", { button: a }); b.init && b.init.call(this, a, d); (b.start || b.steps) && f.renderer.boxWrapper.addClass("highcharts-draw-mode") }; a.prototype.bindingsChartClick =
                        function (a, b) {
                            a = this.chart; var d = this.selectedButton; a = a.renderer.boxWrapper; var f; if (f = this.activeAnnotation && !b.activeAnnotation && b.target.parentNode) { a: { f = b.target; var g = u.Element.prototype, e = g.matches || g.msMatchesSelector || g.webkitMatchesSelector, t = null; if (g.closest) t = g.closest.call(f, ".highcharts-popup"); else { do { if (e.call(f, ".highcharts-popup")) break a; f = f.parentElement || f.parentNode } while (null !== f && 1 === f.nodeType) } f = t } f = !f } f && (c(this, "closePopup"), this.deselectAnnotation()); d && d.start && (this.nextEvent ?
                                (this.nextEvent(b, this.currentUserDetails), this.steps && (this.stepIndex++, d.steps[this.stepIndex] ? this.mouseMoveEvent = this.nextEvent = d.steps[this.stepIndex] : (c(this, "deselectButton", { button: this.selectedButtonElement }), a.removeClass("highcharts-draw-mode"), d.end && d.end.call(this, b, this.currentUserDetails), this.mouseMoveEvent = this.nextEvent = !1, this.selectedButton = null))) : (this.currentUserDetails = d.start.call(this, b), d.steps ? (this.stepIndex = 0, this.steps = !0, this.mouseMoveEvent = this.nextEvent = d.steps[this.stepIndex]) :
                                    (c(this, "deselectButton", { button: this.selectedButtonElement }), a.removeClass("highcharts-draw-mode"), this.steps = !1, this.selectedButton = null, d.end && d.end.call(this, b, this.currentUserDetails))))
                        }; a.prototype.bindingsContainerMouseMove = function (a, b) { this.mouseMoveEvent && this.mouseMoveEvent(b, this.currentUserDetails) }; a.prototype.fieldsToOptions = function (a, b) {
                            g(a, function (a, c) {
                                var d = parseFloat(a), g = c.split("."), e = b, t = g.length - 1; !x(d) || a.match(/px/g) || c.match(/format/g) || (a = d); "" !== a && "undefined" !== a &&
                                    g.forEach(function (b, c) { var d = f(g[c + 1], ""); t === c ? e[b] = a : (e[b] || (e[b] = d.match(/\d/g) ? [] : {}), e = e[b]) })
                            }); return b
                        }; a.prototype.deselectAnnotation = function () { this.activeAnnotation && (this.activeAnnotation.setControlPointsVisibility(!1), this.activeAnnotation = !1) }; a.prototype.annotationToFields = function (b) {
                            function c(a, d, f, e) {
                                if (f && -1 === w.indexOf(d) && (0 <= (f.indexOf && f.indexOf(d)) || f[d] || !0 === f)) if (h(a)) e[d] = [], a.forEach(function (a, b) {
                                    k(a) ? (e[d][b] = {}, g(a, function (a, f) { c(a, f, t[d], e[d][b]) })) : c(a, 0, t[d],
                                        e[d])
                                }); else if (k(a)) { var n = {}; h(e) ? (e.push(n), n[d] = {}, n = n[d]) : e[d] = n; g(a, function (a, b) { c(a, b, 0 === d ? f : t[d], n) }) } else "format" === d ? e[d] = [z(a, b.labels[0].points[0]).toString(), "text"] : h(e) ? e.push([a, l(a)]) : e[d] = [a, l(a)]
                            } var d = b.options, e = a.annotationsEditable, t = e.nestedOptions, l = this.utils.getFieldType, n = f(d.type, d.shapes && d.shapes[0] && d.shapes[0].type, d.labels && d.labels[0] && d.labels[0].itemType, "label"), w = a.annotationsNonEditable[d.langKey] || [], u = { langKey: d.langKey, type: n }; g(d, function (a, b) {
                                "typeOptions" ===
                                b ? (u[b] = {}, g(d[b], function (a, d) { c(a, d, t, u[b], !0) })) : c(a, b, e[n], u)
                            }); return u
                        }; a.prototype.getClickedClassNames = function (a, b) { var c = b.target; b = []; for (var d; c && ((d = p(c, "class")) && (b = b.concat(d.split(" ").map(function (a) { return [a, c] }))), c = c.parentNode, c !== a);); return b }; a.prototype.getButtonEvents = function (a, b) { var c = this, d; this.getClickedClassNames(a, b).forEach(function (a) { c.boundClassNames[a[0]] && !d && (d = { events: c.boundClassNames[a[0]], button: a[1] }) }); return d }; a.prototype.update = function (a) {
                            this.options =
                            y(!0, this.options, a); this.removeEvents(); this.initEvents()
                        }; a.prototype.removeEvents = function () { this.eventsToUnbind.forEach(function (a) { a() }) }; a.prototype.destroy = function () { this.removeEvents() }; a.annotationsEditable = {
                            nestedOptions: {
                                labelOptions: ["style", "format", "backgroundColor"], labels: ["style"], label: ["style"], style: ["fontSize", "color"], background: ["fill", "strokeWidth", "stroke"], innerBackground: ["fill", "strokeWidth", "stroke"], outerBackground: ["fill", "strokeWidth", "stroke"], shapeOptions: ["fill",
                                    "strokeWidth", "stroke"], shapes: ["fill", "strokeWidth", "stroke"], line: ["strokeWidth", "stroke"], backgroundColors: [!0], connector: ["fill", "strokeWidth", "stroke"], crosshairX: ["strokeWidth", "stroke"], crosshairY: ["strokeWidth", "stroke"]
                            }, circle: ["shapes"], verticalLine: [], label: ["labelOptions"], measure: ["background", "crosshairY", "crosshairX"], fibonacci: [], tunnel: ["background", "line", "height"], pitchfork: ["innerBackground", "outerBackground"], rect: ["shapes"], crookedLine: [], basicAnnotation: []
                        }; a.annotationsNonEditable =
                            { rectangle: ["crosshairX", "crosshairY", "label"] }; return a
            }(); l.prototype.utils = { updateRectSize: function (a, b) { var c = b.chart, d = b.options.typeOptions, f = c.pointer.getCoordinates(a); a = f.xAxis[0].value - d.point.x; d = d.point.y - f.yAxis[0].value; b.update({ typeOptions: { background: { width: c.inverted ? d : a, height: c.inverted ? a : d } } }) }, getFieldType: function (a) { return { string: "text", number: "number", "boolean": "checkbox" }[typeof a] } }; m.Chart.prototype.initNavigationBindings = function () {
                var a = this.options; a && a.navigation &&
                    a.navigation.bindings && (this.navigationBindings = new l(this, a.navigation), this.navigationBindings.initEvents(), this.navigationBindings.initUpdate())
            }; b(m.Chart, "load", function () { this.initNavigationBindings() }); b(m.Chart, "destroy", function () { this.navigationBindings && this.navigationBindings.destroy() }); b(l, "deselectButton", function () { this.selectedButtonElement = null }); b(a, "remove", function () { this.chart.navigationBindings && this.chart.navigationBindings.deselectAnnotation() }); m.Annotation && (q(a), g(a.types,
                function (a) { q(a) })); e({
                    lang: {
                        navigation: {
                            popup: {
                                simpleShapes: "Simple shapes", lines: "Lines", circle: "Circle", rectangle: "Rectangle", label: "Label", shapeOptions: "Shape options", typeOptions: "Details", fill: "Fill", format: "Text", strokeWidth: "Line width", stroke: "Line color", title: "Title", name: "Name", labelOptions: "Label options", labels: "Labels", backgroundColor: "Background color", backgroundColors: "Background colors", borderColor: "Border color", borderRadius: "Border radius", borderWidth: "Border width", style: "Style",
                                padding: "Padding", fontSize: "Font size", color: "Color", height: "Height", shapes: "Shape options"
                            }
                        }
                    }, navigation: {
                        bindingsClassName: "highcharts-bindings-container", bindings: {
                            circleAnnotation: {
                                className: "highcharts-circle-annotation", start: function (a) {
                                    a = this.chart.pointer.getCoordinates(a); var b = this.chart.options.navigation; return this.chart.addAnnotation(y({ langKey: "circle", type: "basicAnnotation", shapes: [{ type: "circle", point: { xAxis: 0, yAxis: 0, x: a.xAxis[0].value, y: a.yAxis[0].value }, r: 5 }] }, b.annotationsOptions,
                                        b.bindings.circleAnnotation.annotationsOptions))
                                }, steps: [function (a, b) { var c = b.options.shapes[0].point, d = this.chart.xAxis[0].toPixels(c.x); c = this.chart.yAxis[0].toPixels(c.y); var f = this.chart.inverted; b.update({ shapes: [{ r: Math.max(Math.sqrt(Math.pow(f ? c - a.chartX : d - a.chartX, 2) + Math.pow(f ? d - a.chartY : c - a.chartY, 2)), 5) }] }) }]
                            }, rectangleAnnotation: {
                                className: "highcharts-rectangle-annotation", start: function (a) {
                                    var b = this.chart.pointer.getCoordinates(a); a = this.chart.options.navigation; var c = b.xAxis[0].value;
                                    b = b.yAxis[0].value; return this.chart.addAnnotation(y({ langKey: "rectangle", type: "basicAnnotation", shapes: [{ type: "path", points: [{ xAxis: 0, yAxis: 0, x: c, y: b }, { xAxis: 0, yAxis: 0, x: c, y: b }, { xAxis: 0, yAxis: 0, x: c, y: b }, { xAxis: 0, yAxis: 0, x: c, y: b }] }] }, a.annotationsOptions, a.bindings.rectangleAnnotation.annotationsOptions))
                                }, steps: [function (a, b) { var c = b.options.shapes[0].points, d = this.chart.pointer.getCoordinates(a); a = d.xAxis[0].value; d = d.yAxis[0].value; c[1].x = a; c[2].x = a; c[2].y = d; c[3].y = d; b.update({ shapes: [{ points: c }] }) }]
                            },
                            labelAnnotation: { className: "highcharts-label-annotation", start: function (a) { a = this.chart.pointer.getCoordinates(a); var b = this.chart.options.navigation; return this.chart.addAnnotation(y({ langKey: "label", type: "basicAnnotation", labelOptions: { format: "{y:.2f}" }, labels: [{ point: { xAxis: 0, yAxis: 0, x: a.xAxis[0].value, y: a.yAxis[0].value }, overflow: "none", crop: !0 }] }, b.annotationsOptions, b.bindings.labelAnnotation.annotationsOptions)) } }
                        }, events: {}, annotationsOptions: {}
                    }
                }); return l
    }); p(a, "annotations/popup.js", [a["parts/Globals.js"],
    a["annotations/navigationBindings.js"], a["parts/Pointer.js"], a["parts/Utilities.js"]], function (a, d, m, e) {
        var p = e.addEvent, b = e.createElement, q = e.defined, z = e.getOptions, c = e.isArray, h = e.isObject, r = e.isString, x = e.objectEach, k = e.pick; e = e.wrap; var y = /\d/g; e(m.prototype, "onContainerMouseDown", function (a, b) { var c = b.target && b.target.className; r(c) && 0 <= c.indexOf("highcharts-popup-field") || a.apply(this, Array.prototype.slice.call(arguments, 1)) }); a.Popup = function (a, b) { this.init(a, b) }; a.Popup.prototype = {
            init: function (a,
                c) { this.container = b("div", { className: "highcharts-popup" }, null, a); this.lang = this.getLangpack(); this.iconsURL = c; this.addCloseBtn() }, addCloseBtn: function () { var a = this; var c = b("div", { className: "highcharts-popup-close" }, null, this.container); c.style["background-image"] = "url(" + this.iconsURL + "close.svg)";["click", "touchstart"].forEach(function (b) { p(c, b, function () { a.closePopup() }) }) }, addColsContainer: function (a) {
                    var c = b("div", { className: "highcharts-popup-lhs-col" }, null, a); a = b("div", { className: "highcharts-popup-rhs-col" },
                        null, a); b("div", { className: "highcharts-popup-rhs-col-wrapper" }, null, a); return { lhsCol: c, rhsCol: a }
                }, addInput: function (a, c, d, e) { var f = a.split("."); f = f[f.length - 1]; var g = this.lang; c = "highcharts-" + c + "-" + f; c.match(y) || b("label", { innerHTML: g[f] || f, htmlFor: c }, null, d); b("input", { name: c, value: e[0], type: e[1], className: "highcharts-popup-field" }, null, d).setAttribute("highcharts-data-name", a) }, addButton: function (a, c, d, e, l) {
                    var f = this, g = this.closePopup, n = this.getFields; var h = b("button", { innerHTML: c }, null, a);["click",
                        "touchstart"].forEach(function (a) { p(h, a, function () { g.call(f); return e(n(l, d)) }) }); return h
                }, getFields: function (a, b) {
                    var c = a.querySelectorAll("input"), d = a.querySelectorAll("#highcharts-select-series > option:checked")[0]; a = a.querySelectorAll("#highcharts-select-volume > option:checked")[0]; var f, e; var g = { actionType: b, linkedTo: d && d.getAttribute("value"), fields: {} };[].forEach.call(c, function (a) {
                        e = a.getAttribute("highcharts-data-name"); (f = a.getAttribute("highcharts-data-series-id")) ? g.seriesId = a.value :
                            e ? g.fields[e] = a.value : g.type = a.value
                    }); a && (g.fields["params.volumeSeriesID"] = a.getAttribute("value")); return g
                }, showPopup: function () { var a = this.container, b = a.querySelectorAll(".highcharts-popup-close")[0]; a.innerHTML = ""; 0 <= a.className.indexOf("highcharts-annotation-toolbar") && (a.classList.remove("highcharts-annotation-toolbar"), a.removeAttribute("style")); a.appendChild(b); a.style.display = "block" }, closePopup: function () { this.popup.container.style.display = "none" }, showForm: function (a, b, c, d) {
                    this.popup =
                    b.navigationBindings.popup; this.showPopup(); "indicators" === a && this.indicators.addForm.call(this, b, c, d); "annotation-toolbar" === a && this.annotations.addToolbar.call(this, b, c, d); "annotation-edit" === a && this.annotations.addForm.call(this, b, c, d); "flag" === a && this.annotations.addForm.call(this, b, c, d, !0)
                }, getLangpack: function () { return z().lang.navigation.popup }, annotations: {
                    addToolbar: function (a, c, d) {
                        var f = this, e = this.lang, g = this.popup.container, h = this.showForm; -1 === g.className.indexOf("highcharts-annotation-toolbar") &&
                            (g.className += " highcharts-annotation-toolbar"); g.style.top = a.plotTop + 10 + "px"; b("span", { innerHTML: k(e[c.langKey] || c.langKey, c.shapes && c.shapes[0].type) }, null, g); var n = this.addButton(g, e.removeButton || "remove", "remove", d, g); n.className += " highcharts-annotation-remove-button"; n.style["background-image"] = "url(" + this.iconsURL + "destroy.svg)"; n = this.addButton(g, e.editButton || "edit", "edit", function () { h.call(f, "annotation-edit", a, c, d) }, g); n.className += " highcharts-annotation-edit-button"; n.style["background-image"] =
                                "url(" + this.iconsURL + "edit.svg)"
                    }, addForm: function (a, c, d, e) { var f = this.popup.container, g = this.lang; b("h2", { innerHTML: g[c.langKey] || c.langKey, className: "highcharts-popup-main-title" }, null, f); var n = b("div", { className: "highcharts-popup-lhs-col highcharts-popup-lhs-full" }, null, f); var h = b("div", { className: "highcharts-popup-bottom-row" }, null, f); this.annotations.addFormFields.call(this, n, a, "", c, [], !0); this.addButton(h, e ? g.addButton || "add" : g.saveButton || "save", e ? "add" : "save", d, f) }, addFormFields: function (a,
                        d, e, k, l, t) { var f = this, g = this.annotations.addFormFields, n = this.addInput, u = this.lang, m, p; x(k, function (b, n) { m = "" !== e ? e + "." + n : n; h(b) && (!c(b) || c(b) && h(b[0]) ? (p = u[n] || n, p.match(y) || l.push([!0, p, a]), g.call(f, a, d, m, b, l, !1)) : l.push([f, m, "annotation", a, b])) }); t && (l = l.sort(function (a) { return a[1].match(/format/g) ? -1 : 1 }), l.forEach(function (a) { !0 === a[0] ? b("span", { className: "highcharts-annotation-title", innerHTML: a[1] }, null, a[2]) : n.apply(a[0], a.splice(1)) })) }
                }, indicators: {
                    addForm: function (a, b, c) {
                        var d = this.indicators,
                        f = this.lang; this.tabs.init.call(this, a); b = this.popup.container.querySelectorAll(".highcharts-tab-item-content"); this.addColsContainer(b[0]); d.addIndicatorList.call(this, a, b[0], "add"); var e = b[0].querySelectorAll(".highcharts-popup-rhs-col")[0]; this.addButton(e, f.addButton || "add", "add", c, e); this.addColsContainer(b[1]); d.addIndicatorList.call(this, a, b[1], "edit"); e = b[1].querySelectorAll(".highcharts-popup-rhs-col")[0]; this.addButton(e, f.saveButton || "save", "edit", c, e); this.addButton(e, f.removeButton ||
                            "remove", "remove", c, e)
                    }, addIndicatorList: function (a, c, d) {
                        var e = this, f = c.querySelectorAll(".highcharts-popup-lhs-col")[0]; c = c.querySelectorAll(".highcharts-popup-rhs-col")[0]; var g = "edit" === d, h = g ? a.series : a.options.plotOptions, n = this.indicators.addFormFields, k; var m = b("ul", { className: "highcharts-indicator-list" }, null, f); var q = c.querySelectorAll(".highcharts-popup-rhs-col-wrapper")[0]; x(h, function (c, d) {
                            var f = c.options; if (c.params || f && f.params) {
                                var l = e.indicators.getNameType(c, d), u = l.type; k = b("li", {
                                    className: "highcharts-indicator-list",
                                    innerHTML: l.name
                                }, null, m);["click", "touchstart"].forEach(function (d) { p(k, d, function () { n.call(e, a, g ? c : h[u], l.type, q); g && c.options && b("input", { type: "hidden", name: "highcharts-id-" + u, value: c.options.id }, null, q).setAttribute("highcharts-data-series-id", c.options.id) }) })
                            }
                        }); 0 < m.childNodes.length && m.childNodes[0].click()
                    }, getNameType: function (b, c) { var d = b.options, e = a.seriesTypes; e = e[c] && e[c].prototype.nameBase || c.toUpperCase(); d && d.type && (c = b.options.type, e = b.name); return { name: e, type: c } }, listAllSeries: function (a,
                        c, d, e, h) { a = "highcharts-" + c + "-type-" + a; var f; b("label", { innerHTML: this.lang[c] || c, htmlFor: a }, null, e); var g = b("select", { name: a, className: "highcharts-popup-field" }, null, e); g.setAttribute("id", "highcharts-select-" + c); d.series.forEach(function (a) { f = a.options; !f.params && f.id && "highcharts-navigator-series" !== f.id && b("option", { innerHTML: f.name || f.id, value: f.id }, null, g) }); q(h) && (g.value = h) }, addFormFields: function (a, c, d, e) {
                            var f = c.params || c.options.params, g = this.indicators.getNameType; e.innerHTML = ""; b("h3",
                                { className: "highcharts-indicator-title", innerHTML: g(c, d).name }, null, e); b("input", { type: "hidden", name: "highcharts-type-" + d, value: d }, null, e); this.indicators.listAllSeries.call(this, d, "series", a, e, c.linkedParent && f.volumeSeriesID); f.volumeSeriesID && this.indicators.listAllSeries.call(this, d, "volume", a, e, c.linkedParent && c.linkedParent.options.id); this.indicators.addParamInputs.call(this, a, "params", f, d, e)
                        }, addParamInputs: function (a, b, c, d, e) {
                            var f = this, g = this.indicators.addParamInputs, k = this.addInput, n;
                            x(c, function (c, l) { n = b + "." + l; h(c) ? g.call(f, a, n, c, d, e) : "params.volumeSeriesID" !== n && k.call(f, n, d, e, [c, "text"]) })
                        }, getAmount: function () { var a = 0; this.series.forEach(function (b) { var c = b.options; (b.params || c && c.params) && a++ }); return a }
                }, tabs: {
                    init: function (a) { var b = this.tabs; a = this.indicators.getAmount.call(a); var c = b.addMenuItem.call(this, "add"); b.addMenuItem.call(this, "edit", a); b.addContentItem.call(this, "add"); b.addContentItem.call(this, "edit"); b.switchTabs.call(this, a); b.selectTab.call(this, c, 0) }, addMenuItem: function (a,
                        c) { var d = this.popup.container, e = "highcharts-tab-item", f = this.lang; 0 === c && (e += " highcharts-tab-disabled"); c = b("span", { innerHTML: f[a + "Button"] || a, className: e }, null, d); c.setAttribute("highcharts-data-tab-type", a); return c }, addContentItem: function () { return b("div", { className: "highcharts-tab-item-content" }, null, this.popup.container) }, switchTabs: function (a) {
                            var b = this, c; this.popup.container.querySelectorAll(".highcharts-tab-item").forEach(function (d, e) {
                                c = d.getAttribute("highcharts-data-tab-type"); "edit" ===
                                    c && 0 === a || ["click", "touchstart"].forEach(function (a) { p(d, a, function () { b.tabs.deselectAll.call(b); b.tabs.selectTab.call(b, this, e) }) })
                            })
                        }, selectTab: function (a, b) { var c = this.popup.container.querySelectorAll(".highcharts-tab-item-content"); a.className += " highcharts-tab-item-active"; c[b].className += " highcharts-tab-item-show" }, deselectAll: function () {
                            var a = this.popup.container, b = a.querySelectorAll(".highcharts-tab-item"); a = a.querySelectorAll(".highcharts-tab-item-content"); var c; for (c = 0; c < b.length; c++)b[c].classList.remove("highcharts-tab-item-active"),
                                a[c].classList.remove("highcharts-tab-item-show")
                        }
                }
        }; p(d, "showPopup", function (b) { this.popup || (this.popup = new a.Popup(this.chart.container, this.chart.options.navigation.iconsURL || this.chart.options.stockTools && this.chart.options.stockTools.gui.iconsURL || "https://code.highcharts.com/8.1.2/gfx/stock-icons/")); this.popup.showForm(b.formType, this.chart, b.options, b.onSubmit) }); p(d, "closePopup", function () { this.popup && this.popup.closePopup() })
    }); p(a, "masters/modules/annotations.src.js", [], function () { })
});
//# sourceMappingURL=annotations.js.map