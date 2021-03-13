#pragma once

class CSharpBaseListener;
class ParserRuleContext;

class TreeShapeListener : public CSharpBaseListener
{
public:
    void enterKey(ParserRuleContext *ctx) override;
};
