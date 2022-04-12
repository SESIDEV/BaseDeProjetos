/*
 Highcharts JS v8.1.2 (2020-06-16)

 Sonification module

 (c) 2012-2019 ystein Moseng

 License: www.highcharts.com/license
*/
(function (b) { "object" === typeof module && module.exports ? (b["default"] = b, module.exports = b) : "function" === typeof define && define.amd ? define("highcharts/modules/sonification", ["highcharts"], function (l) { b(l); b.Highcharts = l; return b }) : b("undefined" !== typeof Highcharts ? Highcharts : void 0) })(function (b) {
    function l(d, b, f, n) { d.hasOwnProperty(b) || (d[b] = n.apply(null, f)) } b = b ? b._modules : {}; l(b, "modules/sonification/Instrument.js", [b["parts/Globals.js"], b["parts/Utilities.js"]], function (d, b) {
        function f(c) { this.init(c) }
        var n = b.error, m = b.merge, k = b.pick, x = b.uniqueKey, p = { type: "oscillator", playCallbackInterval: 20, oscillator: { waveformShape: "sine" } }; f.prototype.init = function (c) {
            this.initAudioContext() ? (this.options = m(p, c), this.id = this.options.id = c && c.id || x(), c = d.audioContext, this.gainNode = c.createGain(), this.setGain(0), (this.panNode = c.createStereoPanner && c.createStereoPanner()) ? (this.setPan(0), this.gainNode.connect(this.panNode), this.panNode.connect(c.destination)) : this.gainNode.connect(c.destination), "oscillator" ===
                this.options.type && this.initOscillator(this.options.oscillator), this.playCallbackTimers = []) : n(29)
        }; f.prototype.copy = function (c) { return new f(m(this.options, { id: null }, c)) }; f.prototype.initAudioContext = function () { var c = d.win.AudioContext || d.win.webkitAudioContext, a = !!d.audioContext; return c ? (d.audioContext = d.audioContext || new c, !a && d.audioContext && "running" === d.audioContext.state && d.audioContext.suspend(), !!(d.audioContext && d.audioContext.createOscillator && d.audioContext.createGain)) : !1 }; f.prototype.initOscillator =
            function (c) { this.oscillator = d.audioContext.createOscillator(); this.oscillator.type = c.waveformShape; this.oscillator.connect(this.gainNode); this.oscillatorStarted = !1 }; f.prototype.setPan = function (c) { this.panNode && this.panNode.pan.setValueAtTime(c, d.audioContext.currentTime) }; f.prototype.setGain = function (c, a) {
                this.gainNode && (1.2 < c && (console.warn("Highcharts sonification warning: Volume of instrument set too high."), c = 1.2), a ? (this.gainNode.gain.setValueAtTime(this.gainNode.gain.value, d.audioContext.currentTime),
                    this.gainNode.gain.linearRampToValueAtTime(c, d.audioContext.currentTime + a / 1E3)) : this.gainNode.gain.setValueAtTime(c, d.audioContext.currentTime))
            }; f.prototype.cancelGainRamp = function () { this.gainNode && this.gainNode.gain.cancelScheduledValues(0) }; f.prototype.getValidFrequency = function (c, a, e) { var h = this.options.allowedFrequencies, d = k(e, Infinity), b = k(a, -Infinity); return h && h.length ? h.reduce(function (a, e) { return Math.abs(e - c) < Math.abs(a - c) && e < d && e > b ? e : a }, Infinity) : c }; f.prototype.clearPlayCallbackTimers =
                function () { this.playCallbackTimers.forEach(function (c) { clearInterval(c) }); this.playCallbackTimers = [] }; f.prototype.setFrequency = function (c, a) { a = a || {}; c = this.getValidFrequency(c, a.min, a.max); "oscillator" === this.options.type && this.oscillatorPlay(c) }; f.prototype.oscillatorPlay = function (c) { this.oscillatorStarted || (this.oscillator.start(), this.oscillatorStarted = !0); this.oscillator.frequency.setValueAtTime(c, d.audioContext.currentTime) }; f.prototype.preparePlay = function () {
                    this.setGain(.001); "suspended" ===
                        d.audioContext.state && d.audioContext.resume(); this.oscillator && !this.oscillatorStarted && (this.oscillator.start(), this.oscillatorStarted = !0)
                }; f.prototype.play = function (c) {
                    var a = this, e = c.duration || 0, h = function (e, h, b) { var d = c.duration, f = 0, k = a.options.playCallbackInterval; if ("function" === typeof e) { var m = setInterval(function () { f++; var c = f * k / d; if (1 <= c) a[h](e(1), b), clearInterval(m); else a[h](e(c), b) }, k); a.playCallbackTimers.push(m) } else a[h](e, b) }; if (a.id) if ("suspended" === d.audioContext.state || this.oscillator &&
                        !this.oscillatorStarted) a.preparePlay(), setTimeout(function () { a.play(c) }, 10); else {
                            a.playCallbackTimers.length && a.clearPlayCallbackTimers(); a.cancelGainRamp(); a.stopOscillatorTimeout && (clearTimeout(a.stopOscillatorTimeout), delete a.stopOscillatorTimeout); a.stopTimeout && (clearTimeout(a.stopTimeout), delete a.stopTimeout, a.stopCallback && (a._play = a.play, a.play = function () { }, a.stopCallback("cancelled"), a.play = a._play)); var b = e < d.sonification.fadeOutDuration + 20; a.stopCallback = c.onEnd; var f = function () {
                                delete a.stopTimeout;
                                a.stop(b)
                            }; e ? (a.stopTimeout = setTimeout(f, b ? e : e - d.sonification.fadeOutDuration), h(c.frequency, "setFrequency", { minFrequency: c.minFrequency, maxFrequency: c.maxFrequency }), h(k(c.volume, 1), "setGain", 4), h(k(c.pan, 0), "setPan")) : f()
                    }
                }; f.prototype.mute = function () { this.setGain(.0001, .8 * d.sonification.fadeOutDuration) }; f.prototype.stop = function (c, a, e) {
                    var h = this, b = function () {
                        h.stopOscillatorTimeout && delete h.stopOscillatorTimeout; try { h.oscillator.stop() } catch (t) { } h.oscillator.disconnect(h.gainNode); h.initOscillator(h.options.oscillator);
                        a && a(e); h.stopCallback && h.stopCallback(e)
                    }; h.playCallbackTimers.length && h.clearPlayCallbackTimers(); h.stopTimeout && clearTimeout(h.stopTimeout); c ? (h.setGain(0), b()) : (h.mute(), h.stopOscillatorTimeout = setTimeout(b, d.sonification.fadeOutDuration + 100))
                }; return f
    }); l(b, "modules/sonification/musicalFrequencies.js", [], function () {
        return [16.351597831287414, 17.323914436054505, 18.354047994837977, 19.445436482630058, 20.601722307054366, 21.826764464562746, 23.12465141947715, 24.499714748859326, 25.956543598746574, 27.5,
            29.13523509488062, 30.86770632850775, 32.70319566257483, 34.64782887210901, 36.70809598967594, 38.890872965260115, 41.20344461410875, 43.653528929125486, 46.2493028389543, 48.999429497718666, 51.91308719749314, 55, 58.27047018976124, 61.7354126570155, 65.40639132514966, 69.29565774421802, 73.41619197935188, 77.78174593052023, 82.4068892282175, 87.30705785825097, 92.4986056779086, 97.99885899543733, 103.82617439498628, 110, 116.54094037952248, 123.47082531403103, 130.8127826502993, 138.59131548843604, 146.8323839587038, 155.56349186104046,
            164.81377845643496, 174.61411571650194, 184.9972113558172, 195.99771799087463, 207.65234878997256, 220, 233.08188075904496, 246.94165062806206, 261.6255653005986, 277.1826309768721, 293.6647679174076, 311.1269837220809, 329.6275569128699, 349.2282314330039, 369.9944227116344, 391.99543598174927, 415.3046975799451, 440, 466.1637615180899, 493.8833012561241, 523.2511306011972, 554.3652619537442, 587.3295358348151, 622.2539674441618, 659.2551138257398, 698.4564628660078, 739.9888454232688, 783.9908719634985, 830.6093951598903,
            880, 932.3275230361799, 987.7666025122483, 1046.5022612023945, 1108.7305239074883, 1174.6590716696303, 1244.5079348883237, 1318.5102276514797, 1396.9129257320155, 1479.9776908465376, 1567.981743926997, 1661.2187903197805, 1760, 1864.6550460723597, 1975.533205024496, 2093.004522404789, 2217.4610478149766, 2349.31814333926, 2489.0158697766474, 2637.02045530296, 2793.825851464031, 2959.955381693075, 3135.9634878539946, 3322.437580639561, 3520, 3729.3100921447194, 3951.066410048992, 4186.009044809578]
    }); l(b, "modules/sonification/utilities.js",
        [b["modules/sonification/musicalFrequencies.js"], b["parts/Utilities.js"]], function (b, g) {
            function d(b) { this.init(b || []) } var n = g.clamp; d.prototype.init = function (b) { this.supportedSignals = b; this.signals = {} }; d.prototype.registerSignalCallbacks = function (b) { var d = this; d.supportedSignals.forEach(function (f) { var k = b[f]; k && (d.signals[f] = d.signals[f] || []).push(k) }) }; d.prototype.clearSignalCallbacks = function (b) { var d = this; b ? b.forEach(function (b) { d.signals[b] && delete d.signals[b] }) : d.signals = {} }; d.prototype.emitSignal =
                function (b, d) { var f; this.signals[b] && this.signals[b].forEach(function (b) { b = b(d); f = "undefined" !== typeof b ? b : f }); return f }; return {
                    musicalFrequencies: b, SignalHandler: d, getMusicalScale: function (d) { return b.filter(function (b, f) { var k = f % 12 + 1; return d.some(function (c) { return c === k }) }) }, calculateDataExtremes: function (b, d) {
                        return b.series.reduce(function (b, f) { f.points.forEach(function (c) { c = "undefined" !== typeof c[d] ? c[d] : c.options[d]; b.min = Math.min(b.min, c); b.max = Math.max(b.max, c) }); return b }, {
                            min: Infinity,
                            max: -Infinity
                        })
                    }, virtualAxisTranslate: function (b, d, f) { var k = d.max - d.min; b = f.min + (f.max - f.min) * (b - d.min) / k; return 0 < k ? n(b, f.min, f.max) : f.min }
                }
        }); l(b, "modules/sonification/instrumentDefinitions.js", [b["modules/sonification/Instrument.js"], b["modules/sonification/utilities.js"]], function (b, g) {
            var d = {};["sine", "square", "triangle", "sawtooth"].forEach(function (f) {
                d[f] = new b({ oscillator: { waveformShape: f } }); d[f + "Musical"] = new b({ allowedFrequencies: g.musicalFrequencies, oscillator: { waveformShape: f } }); d[f + "Major"] =
                    new b({ allowedFrequencies: g.getMusicalScale([1, 3, 5, 6, 8, 10, 12]), oscillator: { waveformShape: f } })
            }); return d
        }); l(b, "modules/sonification/Earcon.js", [b["parts/Globals.js"], b["parts/Utilities.js"]], function (b, g) {
            function d(b) { this.init(b || {}) } var n = g.error, m = g.merge, k = g.pick, l = g.uniqueKey; d.prototype.init = function (b) { this.options = b; this.options.id || (this.options.id = this.id = l()); this.instrumentsPlaying = {} }; d.prototype.sonify = function (d) {
                var c = m(this.options, d), a = k(c.volume, 1), e = c.pan, h = this, f = d && d.onEnd,
                t = h.options.onEnd; c.instruments.forEach(function (c) {
                    var d = "string" === typeof c.instrument ? b.sonification.instruments[c.instrument] : c.instrument, r = m(c.playOptions), g = ""; if (d && d.play) {
                        if (c.playOptions) {
                            "function" !== typeof c.playOptions.volume && (r.volume = k(a, 1) * k(c.playOptions.volume, 1)); r.pan = k(e, r.pan); var q = r.onEnd; r.onEnd = function () { delete h.instrumentsPlaying[g]; q && q.apply(this, arguments); Object.keys(h.instrumentsPlaying).length || (f && f.apply(this, arguments), t && t.apply(this, arguments)) }; c = d.copy();
                            g = c.id; h.instrumentsPlaying[g] = c; c.play(r)
                        }
                    } else n(30)
                })
            }; d.prototype.cancelSonify = function (b) { var c = this.instrumentsPlaying, a = c && Object.keys(c); a && a.length && (a.forEach(function (a) { c[a].stop(!b, null, "cancelled") }), this.instrumentsPlaying = {}) }; return d
        }); l(b, "modules/sonification/pointSonify.js", [b["parts/Globals.js"], b["parts/Utilities.js"], b["modules/sonification/utilities.js"]], function (b, g, f) {
            var d = g.error, m = g.merge, k = g.pick, l = {
                minDuration: 20, maxDuration: 2E3, minVolume: .1, maxVolume: 1, minPan: -1,
                maxPan: 1, minFrequency: 220, maxFrequency: 2200
            }; return {
                pointSonify: function (g) {
                    var c = this, a = c.series.chart, e = g.dataExtremes || {}, h = function (a, b, d) { return "function" === typeof a ? b ? function (b) { return a(c, e, b) } : a(c, e) : "string" === typeof a ? (e[a] = e[a] || f.calculateDataExtremes(c.series.chart, a), f.virtualAxisTranslate(k(c[a], c.options[a]), e[a], d)) : a }; a.sonification.currentlyPlayingPoint = c; c.sonification = c.sonification || {}; c.sonification.instrumentsPlaying = c.sonification.instrumentsPlaying || {}; var q = c.sonification.signalHandler =
                        c.sonification.signalHandler || new f.SignalHandler(["onEnd"]); q.clearSignalCallbacks(); q.registerSignalCallbacks({ onEnd: g.onEnd }); !c.isNull && c.visible && c.series.visible ? g.instruments.forEach(function (e) {
                            var f = "string" === typeof e.instrument ? b.sonification.instruments[e.instrument] : e.instrument, k = e.instrumentMapping || {}, g = m(l, e.instrumentOptions), n = f.id, t = function (b) {
                                e.onEnd && e.onEnd.apply(this, arguments); a.sonification && a.sonification.currentlyPlayingPoint && delete a.sonification.currentlyPlayingPoint;
                                c.sonification && c.sonification.instrumentsPlaying && (delete c.sonification.instrumentsPlaying[n], Object.keys(c.sonification.instrumentsPlaying).length || q.emitSignal("onEnd", b))
                            }; f && f.play ? (c.sonification.instrumentsPlaying[f.id] = f, f.play({
                                frequency: h(k.frequency, !0, { min: g.minFrequency, max: g.maxFrequency }), duration: h(k.duration, !1, { min: g.minDuration, max: g.maxDuration }), pan: h(k.pan, !0, { min: g.minPan, max: g.maxPan }), volume: h(k.volume, !0, { min: g.minVolume, max: g.maxVolume }), onEnd: t, minFrequency: g.minFrequency,
                                maxFrequency: g.maxFrequency
                            })) : d(30)
                        }) : q.emitSignal("onEnd")
                }, pointCancelSonify: function (b) { var c = this.sonification && this.sonification.instrumentsPlaying, a = c && Object.keys(c); a && a.length && (a.forEach(function (a) { c[a].stop(!b, null, "cancelled") }), this.sonification.instrumentsPlaying = {}, this.sonification.signalHandler.emitSignal("onEnd", "cancelled")) }
            }
        }); l(b, "modules/sonification/chartSonify.js", [b["parts/Globals.js"], b["parts/Point.js"], b["parts/Utilities.js"], b["modules/sonification/utilities.js"]],
            function (b, g, f, n) {
                function d(a, b) { return "function" === typeof b ? b(a) : y(a[b], a.options[b]) } function k(a, b) { return a.points.reduce(function (a, c) { c = d(c, b); a.min = Math.min(a.min, c); a.max = Math.max(a.max, c); return a }, { min: Infinity, max: -Infinity }) } function l(a, b, c) { return (b || []).reduce(function (b, c) { Object.keys(c.instrumentMapping || {}).forEach(function (d) { d = c.instrumentMapping[d]; "string" !== typeof d || b[d] || (b[d] = n.calculateDataExtremes(a, d)) }); return b }, w(c)) } function p(a, c) {
                    return c.reduce(function (c, d) {
                        var e =
                            d.earcon; d.condition ? (d = d.condition(a), d instanceof b.sonification.Earcon ? c.push(d) : d && c.push(e)) : d.onPoint && a.id === d.onPoint && c.push(e); return c
                    }, [])
                } function c(a) { return a.map(function (a) { var c = a.instrument; c = ("string" === typeof c ? b.sonification.instruments[c] : c).copy(); return w(a, { instrument: c }) }) } function a(a, e) {
                    var f = e.timeExtremes || k(a, e.pointPlayTime), h = l(a.chart, e.instruments, e.dataExtremes), C = c(e.instruments), D = a.points.reduce(function (a, c) {
                        var k = p(c, e.earcons || []), g = n.virtualAxisTranslate(d(c,
                            e.pointPlayTime), f, { min: 0, max: e.duration }); return a.concat(new b.sonification.TimelineEvent({ eventObject: c, time: g, id: c.id, playOptions: { instruments: C, dataExtremes: h } }), k.map(function (a) { return new b.sonification.TimelineEvent({ eventObject: a, time: g }) }))
                    }, []); return new b.sonification.TimelinePath({
                        events: D, onStart: function () { if (e.onStart) e.onStart(a) }, onEventStart: function (a) {
                            var b = a.options && a.options.eventObject; if (b instanceof g) {
                                if (!b.series.visible && !b.series.chart.series.some(function (a) { return a.visible })) return a.timelinePath.timeline.pause(),
                                    a.timelinePath.timeline.resetCursor(), !1; if (e.onPointStart) e.onPointStart(a, b)
                            }
                        }, onEventEnd: function (a) { var b = a.event && a.event.options && a.event.options.eventObject; if (b instanceof g && e.onPointEnd) e.onPointEnd(a.event, b) }, onEnd: function () { if (e.onEnd) e.onEnd(a) }
                    })
                } function e(a, b, c) {
                    var d = c.seriesOptions || {}; return w({ dataExtremes: b, timeExtremes: k(a, c.pointPlayTime), instruments: c.instruments, onStart: c.onSeriesStart, onEnd: c.onSeriesEnd, earcons: c.earcons }, B(d) ? A(d, function (b) { return b.id === y(a.id, a.options.id) }) ||
                        {} : d, { pointPlayTime: c.pointPlayTime })
                } function h(a, c, d) {
                    if ("sequential" === a || "simultaneous" === a) { var e = c.series.reduce(function (a, b) { b.visible && a.push({ series: b, seriesOptions: d(b) }); return a }, []); "simultaneous" === a && (e = [e]) } else e = a.reduce(function (a, e) {
                        e = u(e).reduce(function (a, e) {
                            var f; if ("string" === typeof e) { var h = c.get(e); h.visible && (f = { series: h, seriesOptions: d(h) }) } else e instanceof b.sonification.Earcon && (f = new b.sonification.TimelinePath({ events: [new b.sonification.TimelineEvent({ eventObject: e })] }));
                            e.silentWait && (f = new b.sonification.TimelinePath({ silentWait: e.silentWait })); f && a.push(f); return a
                        }, []); e.length && a.push(e); return a
                    }, []); return e
                } function q(a, c) { return c ? a.reduce(function (e, d, f) { d = u(d); e.push(d); f < a.length - 1 && d.some(function (a) { return a.series }) && e.push(new b.sonification.TimelinePath({ silentWait: c })); return e }, []) : a } function t(a) { return a.reduce(function (a, b) { b = u(b); return a + (1 === b.length && b[0].options && b[0].options.silentWait || 0) }, 0) } function v(a) {
                    var c = a.reduce(function (a, b) {
                        (b =
                            b.events) && b.length && (a.min = Math.min(b[0].time, a.min), a.max = Math.max(b[b.length - 1].time, a.max)); return a
                    }, { min: Infinity, max: -Infinity }); a.forEach(function (a) { var e = a.events, d = e && e.length, f = []; d && e[0].time <= c.min || f.push(new b.sonification.TimelineEvent({ time: c.min })); d && e[e.length - 1].time >= c.max || f.push(new b.sonification.TimelineEvent({ time: c.max })); f.length && a.addTimelineEvents(f) })
                } function z(a) {
                    return a.reduce(function (a, b) {
                        return a + u(b).reduce(function (a, b) {
                            return (b = b.series && b.seriesOptions &&
                                b.seriesOptions.timeExtremes) ? Math.max(a, b.max - b.min) : a
                        }, 0)
                    }, 0)
                } function r(c, e) { var d = Math.max(e - t(c), 0), f = z(c); return c.reduce(function (c, e) { e = u(e).reduce(function (c, e) { e instanceof b.sonification.TimelinePath ? c.push(e) : e.series && (e.seriesOptions.duration = e.seriesOptions.duration || n.virtualAxisTranslate(e.seriesOptions.timeExtremes.max - e.seriesOptions.timeExtremes.min, { min: 0, max: f }, { min: 0, max: d }), c.push(a(e.series, e.seriesOptions))); return c }, []); c.push(e); return c }, []) } ""; var A = f.find, B = f.isArray,
                    w = f.merge, y = f.pick, u = f.splat; return {
                        chartSonify: function (a) { var c = w(this.options.sonification, a); this.sonification.timeline && this.sonification.timeline.pause(); this.sonification.duration = c.duration; var d = l(this, c.instruments, c.dataExtremes); a = h(c.order, this, function (a) { return e(a, d, c) }); a = q(a, c.afterSeriesWait || 0); a = r(a, c.duration); a.forEach(function (a) { v(a) }); this.sonification.timeline = new b.sonification.Timeline({ paths: a, onEnd: c.onEnd }); this.sonification.timeline.play() }, seriesSonify: function (c) {
                            var e =
                                a(this, c), d = this.chart.sonification; d.timeline && d.timeline.pause(); d.duration = c.duration; d.timeline = new b.sonification.Timeline({ paths: [e] }); d.timeline.play()
                        }, pause: function (a) { this.sonification.timeline ? this.sonification.timeline.pause(y(a, !0)) : this.sonification.currentlyPlayingPoint && this.sonification.currentlyPlayingPoint.cancelSonify(a) }, resume: function (a) { this.sonification.timeline && this.sonification.timeline.play(a) }, rewind: function (a) { this.sonification.timeline && this.sonification.timeline.rewind(a) },
                        cancel: function (a) { this.pauseSonify(a); this.resetSonifyCursor() }, getCurrentPoints: function () { if (this.sonification.timeline) { var a = this.sonification.timeline.getCursor(); return Object.keys(a).map(function (b) { return a[b].eventObject }).filter(function (a) { return a instanceof g }) } return [] }, setCursor: function (a) { var b = this.sonification.timeline; b && u(a).forEach(function (a) { b.setCursor(a.id) }) }, resetCursor: function () { this.sonification.timeline && this.sonification.timeline.resetCursor() }, resetCursorEnd: function () {
                            this.sonification.timeline &&
                            this.sonification.timeline.resetCursorEnd()
                        }
                    }
            }); l(b, "modules/sonification/Timeline.js", [b["parts/Globals.js"], b["parts/Utilities.js"], b["modules/sonification/utilities.js"]], function (b, g, f) {
                function d(a) { this.init(a || {}) } function m(a) { this.init(a) } function k(a) { this.init(a || {}) } var l = g.merge, p = g.splat, c = g.uniqueKey; d.prototype.init = function (a) { this.options = a; this.time = a.time || 0; this.id = this.options.id = a.id || c() }; d.prototype.play = function (a) {
                    var b = this.options.eventObject, c = this.options.onEnd, d = a &&
                        a.onEnd, f = this.options.playOptions && this.options.playOptions.onEnd; a = l(this.options.playOptions, a); b && b.sonify ? (a.onEnd = c || d || f ? function () { var a = arguments;[c, d, f].forEach(function (b) { b && b.apply(this, a) }) } : void 0, b.sonify(a)) : (d && d(), c && c())
                }; d.prototype.cancel = function (a) { this.options.eventObject.cancelSonify(a) }; m.prototype.init = function (a) {
                    this.options = a; this.id = this.options.id = a.id || c(); this.cursor = 0; this.eventsPlaying = {}; this.events = a.silentWait ? [new d({ time: 0 }), new d({ time: a.silentWait })] : this.options.events;
                    this.sortEvents(); this.updateEventIdMap(); this.signalHandler = new f.SignalHandler(["playOnEnd", "masterOnEnd", "onStart", "onEventStart", "onEventEnd"]); this.signalHandler.registerSignalCallbacks(l(a, { masterOnEnd: a.onEnd }))
                }; m.prototype.sortEvents = function () { this.events = this.events.sort(function (a, b) { return a.time - b.time }) }; m.prototype.updateEventIdMap = function () { this.eventIdMap = this.events.reduce(function (a, b, c) { a[b.id] = c; return a }, {}) }; m.prototype.addTimelineEvents = function (a) {
                    this.events = this.events.concat(a);
                    this.sortEvents(); this.updateEventIdMap()
                }; m.prototype.getCursor = function () { return this.events[this.cursor] }; m.prototype.setCursor = function (a) { a = this.eventIdMap[a]; return "undefined" !== typeof a ? (this.cursor = a, !0) : !1 }; m.prototype.play = function (a) { this.pause(); this.signalHandler.emitSignal("onStart"); this.signalHandler.clearSignalCallbacks(["playOnEnd"]); this.signalHandler.registerSignalCallbacks({ playOnEnd: a }); this.playEvents(1) }; m.prototype.rewind = function (a) {
                    this.pause(); this.signalHandler.emitSignal("onStart");
                    this.signalHandler.clearSignalCallbacks(["playOnEnd"]); this.signalHandler.registerSignalCallbacks({ playOnEnd: a }); this.playEvents(-1)
                }; m.prototype.resetCursor = function () { this.cursor = 0 }; m.prototype.resetCursorEnd = function () { this.cursor = this.events.length - 1 }; m.prototype.pause = function (a) { var b = this; clearTimeout(b.nextScheduledPlay); Object.keys(b.eventsPlaying).forEach(function (c) { b.eventsPlaying[c] && b.eventsPlaying[c].cancel(a) }); b.eventsPlaying = {} }; m.prototype.playEvents = function (a) {
                    var b = this, c = b.events[this.cursor],
                    d = b.events[this.cursor + a], f = function (a) { b.signalHandler.emitSignal("masterOnEnd", a); b.signalHandler.emitSignal("playOnEnd", a) }; c.timelinePath = b; if (!1 === b.signalHandler.emitSignal("onEventStart", c)) f({ event: c, cancelled: !0 }); else if (b.eventsPlaying[c.id] = c, c.play({ onEnd: function (a) { a = { event: c, cancelled: !!a }; delete b.eventsPlaying[c.id]; b.signalHandler.emitSignal("onEventEnd", a); d || f(a) } }), d) {
                        var g = Math.abs(d.time - c.time); 1 > g ? (b.cursor += a, b.playEvents(a)) : this.nextScheduledPlay = setTimeout(function () {
                            b.cursor +=
                            a; b.playEvents(a)
                        }, g)
                    }
                }; k.prototype.init = function (a) { this.options = a; this.cursor = 0; this.paths = a.paths; this.pathsPlaying = {}; this.signalHandler = new f.SignalHandler(["playOnEnd", "masterOnEnd", "onPathStart", "onPathEnd"]); this.signalHandler.registerSignalCallbacks(l(a, { masterOnEnd: a.onEnd })) }; k.prototype.play = function (a) { this.pause(); this.signalHandler.clearSignalCallbacks(["playOnEnd"]); this.signalHandler.registerSignalCallbacks({ playOnEnd: a }); this.playPaths(1) }; k.prototype.rewind = function (a) {
                    this.pause();
                    this.signalHandler.clearSignalCallbacks(["playOnEnd"]); this.signalHandler.registerSignalCallbacks({ playOnEnd: a }); this.playPaths(-1)
                }; k.prototype.playPaths = function (a) {
                    var c = p(this.paths[this.cursor]), d = this.paths[this.cursor + a], f = this, g = this.signalHandler, k = 0, m = function (b) {
                        g.emitSignal("onPathStart", b); f.pathsPlaying[b.id] = b; b[0 < a ? "play" : "rewind"](function (e) {
                            e = e && e.cancelled; var h = { path: b, cancelled: e }; delete f.pathsPlaying[b.id]; g.emitSignal("onPathEnd", h); k++; k >= c.length && (d && !e ? (f.cursor += a, p(d).forEach(function (b) {
                                b[0 <
                                    a ? "resetCursor" : "resetCursorEnd"]()
                            }), f.playPaths(a)) : (g.emitSignal("playOnEnd", h), g.emitSignal("masterOnEnd", h)))
                        })
                    }; c.forEach(function (a) { a && (a.timeline = f, setTimeout(function () { m(a) }, b.sonification.fadeOutDuration)) })
                }; k.prototype.pause = function (a) { var b = this; Object.keys(b.pathsPlaying).forEach(function (c) { b.pathsPlaying[c] && b.pathsPlaying[c].pause(a) }); b.pathsPlaying = {} }; k.prototype.resetCursor = function () { this.paths.forEach(function (a) { p(a).forEach(function (a) { a.resetCursor() }) }); this.cursor = 0 };
                k.prototype.resetCursorEnd = function () { this.paths.forEach(function (a) { p(a).forEach(function (a) { a.resetCursorEnd() }) }); this.cursor = this.paths.length - 1 }; k.prototype.setCursor = function (a) { return this.paths.some(function (b) { return p(b).some(function (b) { return b.setCursor(a) }) }) }; k.prototype.getCursor = function () { return this.getCurrentPlayingPaths().reduce(function (a, b) { a[b.id] = b.getCursor(); return a }, {}) }; k.prototype.atStart = function () { return !this.getCurrentPlayingPaths().some(function (a) { return a.cursor }) };
                k.prototype.getCurrentPlayingPaths = function () { return p(this.paths[this.cursor]) }; return { TimelineEvent: d, TimelinePath: m, Timeline: k }
            }); l(b, "modules/sonification/options.js", [], function () { return { sonification: { enabled: !1, duration: 2E3, afterSeriesWait: 1E3, order: "sequential", pointPlayTime: "x", instruments: [{ instrument: "sineMusical", instrumentMapping: { duration: 400, frequency: "y", volume: .7 }, instrumentOptions: { minFrequency: 392, maxFrequency: 1046 } }] } } }); l(b, "modules/sonification/sonification.js", [b["parts/Globals.js"],
            b["parts/Options.js"], b["parts/Point.js"], b["parts/Utilities.js"], b["modules/sonification/Instrument.js"], b["modules/sonification/instrumentDefinitions.js"], b["modules/sonification/Earcon.js"], b["modules/sonification/pointSonify.js"], b["modules/sonification/chartSonify.js"], b["modules/sonification/utilities.js"], b["modules/sonification/Timeline.js"], b["modules/sonification/options.js"]], function (b, g, f, l, m, k, x, p, c, a, e, h) {
                g = g.defaultOptions; var d = l.addEvent, n = l.extend, v = l.merge; b.sonification = {
                    fadeOutDuration: 20,
                    utilities: a, Instrument: m, instruments: k, Earcon: x, TimelineEvent: e.TimelineEvent, TimelinePath: e.TimelinePath, Timeline: e.Timeline
                }; v(!0, g, h); f.prototype.sonify = p.pointSonify; f.prototype.cancelSonify = p.pointCancelSonify; b.Series.prototype.sonify = c.seriesSonify; n(b.Chart.prototype, { sonify: c.chartSonify, pauseSonify: c.pause, resumeSonify: c.resume, rewindSonify: c.rewind, cancelSonify: c.cancel, getCurrentSonifyPoints: c.getCurrentPoints, setSonifyCursor: c.setCursor, resetSonifyCursor: c.resetCursor, resetSonifyCursorEnd: c.resetCursorEnd });
                d(b.Chart, "init", function () { this.sonification = {} }); d(b.Chart, "update", function (a) { (a = a.options.sonification) && v(!0, this.options.sonification, a) })
            }); l(b, "masters/modules/sonification.src.js", [], function () { })
});
//# sourceMappingURL=sonification.js.map