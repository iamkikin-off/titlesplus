using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace TitlesPlus;

public class SteamNetwork : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var p2pPacket = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "_read_P2P_Packet"},
            t => t.Type is TokenType.Newline,
            t => t is IdentifierToken {Name: "PACKET_SIZE"},
            t => t.Type is TokenType.CfMatch,
            t => t is IdentifierToken {Name: "type"},
            t => t.Type is TokenType.Colon,
            t => t.Type is TokenType.Newline,
        ], allowPartialMatch: true);
        
        foreach (var token in tokens) {
            if (p2pPacket.Check(token)) {
                Console.WriteLine("found p2pPacket");
                yield return token;
                
                // "update_title":
                yield return new ConstantToken(new StringVariant("update_title"));
                yield return new Token(TokenType.Colon);

                yield return new Token(TokenType.Newline, 4);

                // var titles_plus = get_node("/root/TitlesPlus")
                yield return new Token(TokenType.PrVar);
                yield return new IdentifierToken("titles_plus");
                
                yield return new Token(TokenType.OpAssign);
                
                yield return new IdentifierToken("get_node");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("/root/TitlesPlus"));
                yield return new Token(TokenType.ParenthesisClose);
                
                // New Line
                yield return new Token(TokenType.Newline, 4);
                
                yield return new IdentifierToken("titles_plus");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("update_title_packet");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("PACKET_SENDER");
                yield return new Token(TokenType.Comma);
                yield return new IdentifierToken("DATA");
                yield return new Token(TokenType.ParenthesisClose);
            } else {
                yield return token;
            }
        }
    }
}