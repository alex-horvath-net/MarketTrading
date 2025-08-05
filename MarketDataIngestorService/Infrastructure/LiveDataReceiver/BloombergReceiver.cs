//using MarketDataIngestionService.Domain;
//using MarketDataIngestionService.Features.LiveMarketData;
//namespace MarketDataIngestionService.Infrastructure.LiveDataReceiver;

//public class BloombergClient : IMarketDataProvider {
//    private Session? _session;
//    private const string BloombergService = "//blp/mktdata";

//    public Task RunForeverorUntilCanceled(string[] symbols, Action<MarketPrice> onMessage, CancellationToken cancellationToken) {
//        return Task.Run(() => {
//            var sessionOptions = new SessionOptions {
//                ServerHost = "localhost",  // or from config
//                ServerPort = 8194
//            };

//            _session = new Session(sessionOptions, (Event evt, Session sess) => {
//                foreach (Message msg in evt) {
//                    try {
//                        string symbol = msg.TopicName?.ToString() ?? "UNKNOWN";
//                        double? bid = msg.HasElement("BID") ? msg.GetElementAsFloat64("BID") : null;
//                        double? ask = msg.HasElement("ASK") ? msg.GetElementAsFloat64("ASK") : null;
//                        double? last = msg.HasElement("LAST_PRICE") ? msg.GetElementAsFloat64("LAST_PRICE") : null;

//                        if (bid is null || ask is null || last is null)
//                            continue;

//                        var price = new MarketPrice {
//                            Name = symbol,
//                            Bid = bid.Value,
//                            Ask = ask.Value,
//                            Last = last.Value,
//                            Timestamp = DateTime.UtcNow,
//                            CorrelationId = Guid.NewGuid().ToString()
//                        };

//                        onMessage(price);
//                    } catch { /* log? */ }
//                }
//            });

//            if (!_session.StartReceiveLiveDataContinuesly() || !_session.OpenService(BloombergService))
//                return;

//            var subscriptions = new SubscriptionList();
//            for (int i = 0; i < symbols.Length; i++) {
//                subscriptions.Add(symbols[i], "BID,ASK,LAST_PRICE", new CorrelationID(i + 1));
//            }

//            _session.Subscribe(subscriptions);

//            while (!cancellationToken.IsCancellationRequested)
//                Thread.Sleep(1000);

//            _session.Stop();
//        }, cancellationToken);
//    }
//}

